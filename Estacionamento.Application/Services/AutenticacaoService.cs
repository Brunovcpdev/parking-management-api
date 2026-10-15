using BCrypt.Net;
using Estacionamento.Application.DTos.Requests;
using Estacionamento.Application.DTOs.Request;
using Estacionamento.Application.DTOs.Response;
using Estacionamento.Application.Interfaces;
using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Exceptions;
using Estacionamento.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace Estacionamento.Application.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AutenticacaoService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task RegistrarAsync(RegistrarUsuarioRequest request)
        {
            var login = request.Login.Trim().ToLowerInvariant();

            var usuarioExistente = await _usuarioRepository.ObterPorLoginAsync(login);
            if (usuarioExistente is not null)
                throw new UsuarioJaExisteException(login);

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var usuario = new Usuario(request.Login, senhaHash, request.Role);

            await _usuarioRepository.AdicionarAsync(usuario);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var login = request.Login.Trim().ToLowerInvariant();
            
            var usuario = await _usuarioRepository.ObterPorLoginAsync(login);
            if (usuario is null)
                throw new CredenciaisInvalidasException();

            var senhaValida = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash);
                if (!senhaValida)
                throw new CredenciaisInvalidasException();

            var expiraEm = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpiraMinutos"));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Login),
                new Claim(ClaimTypes.Role, usuario.Role.ToString())
            };

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiraEm,
                signingCredentials: credenciais);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponse
            {
                Token = tokenString,
                Login = usuario.Login,
                Role = usuario.Role.ToString(),
                ExpiraEm = expiraEm
            };


        }
    }
}
