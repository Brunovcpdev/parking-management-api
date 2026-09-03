using Estacionamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Estacionamento.Application.Interfaces;
using Estacionamento.Application.Services;
using Estacionamento.Domain.Interfaces;
using Estacionamento.Infrastructure.Repositories;

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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
