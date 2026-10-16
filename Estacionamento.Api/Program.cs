using Estacionamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Estacionamento.Application.Interfaces;
using Estacionamento.Application.Services;
using Estacionamento.Domain.Interfaces;
using Estacionamento.Infrastructure.Repositories;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Estacionamento.Application.DTos.Requests;
using Estacionamento.Domain.Enums;
using Estacionamento.Domain.Exceptions;
using Estacionamento.Application.DTOs.Request;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IVagaRepository, VagaRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>(); //Por que AddScoped - Isso é sobre o ciclo de vida do objeto,
builder.Services.AddScoped<IVeiculoRepository, VeiculoRepository>(); //cria uma instância por requisição HTTP, reaproveitada durante toda aquela requisição, e descartada no final.
builder.Services.AddScoped<IEntradaService, EntradaService>();
builder.Services.AddScoped<ITabelaPrecoRepository, TabelaPrecoRepository>();
builder.Services.AddScoped<ICalculadoraPrecoService, CalculadoraPrecoService>();
builder.Services.AddScoped<ISaidaService, SaidaService>();
builder.Services.AddScoped<IVagaConsultaService, VagaConsultaService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Cole apenas o token JWT, sem a palavra Bearer."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var escopo = app.Services.CreateScope())
{
    try
    {
        var configuracao = escopo.ServiceProvider.GetRequiredService<IConfiguration>();
        var loginAdmin = configuracao["AdminInicial:Login"];
        var senhaAdmin = configuracao["AdminInicial:Senha"];

        if(string.IsNullOrWhiteSpace(loginAdmin) || string.IsNullOrWhiteSpace(senhaAdmin))
        {
            app.Logger.LogWarning("Seed do admin inicial ignorado: configure o AdminInicial:Login e AdminInicial:Senha");
        }
        else
        {
            var autenticacaoService = escopo.ServiceProvider.GetRequiredService<IAutenticacaoService>();

            await autenticacaoService.RegistrarAsync(new RegistrarUsuarioRequest
            {
                Login = loginAdmin,
                Senha = senhaAdmin,
                Role = RoleUsuario.Admin
            });

            app.Logger.LogInformation("Admin inicial '{Login}' criado.", loginAdmin);
        }
    }
    catch(UsuarioJaExisteException)
    {
        //Já existe: o seed é idempotente, não há o que fazer.
    }
    catch(Exception ex)
    {
        app.Logger.LogWarning(ex, "Seed do admin inicial não iniciado");
    }
    
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
