using Estacionamento.Application.DTos.Requests;
using Estacionamento.Application.DTOs.Request;
using Estacionamento.Application.Services;
using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;
using Estacionamento.Domain.Exceptions;
using Estacionamento.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;


namespace Estacionamento.Tests;

public class AutenticacaoServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock = new();
    private readonly AutenticacaoService _service;

    public AutenticacaoServiceTests()
    {
        var configuracao = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "chave-apenas-para-testes-com-mais-de-32-caracteres",
                ["Jwt:Issuer"] = "TesteIssuer",
                ["Jwt:Audience"] = "TesteAudience",
                ["Jwt:ExpiraMinutos"] = "60"
            })
            .Build();

        _service = new AutenticacaoService(_usuarioRepositoryMock.Object, configuracao);
    }

    [Fact]

    public async Task Registrar_LoginJaExiste_LancaUsuarioJaExisteException()
    {
        //Arrange

        var usuarioExistente = new Usuario("admin", "QualQuerHash", RoleUsuario.Admin);

        _usuarioRepositoryMock
            .Setup(r => r.ObterPorLoginAsync(It.IsAny<string>()))
            .ReturnsAsync(usuarioExistente);

        var request = new RegistrarUsuarioRequest
        {
            Login = "admin",
            Senha = "senha123",
            Role = RoleUsuario.Operador
        };

        //ACT + ASSERT
        await Assert.ThrowsAsync<UsuarioJaExisteException>(() => _service.RegistrarAsync(request));
    }
    [Fact]

    public async Task Registrar_LoginNovo_SalvaUsuarioComHashENormalizado()
    {
        //Arrange
        _usuarioRepositoryMock
            .Setup(r => r.ObterPorLoginAsync(It.IsAny<string>()))
            .ReturnsAsync((Usuario?)null);

        Usuario? usuarioSalvo = null;

        _usuarioRepositoryMock
            .Setup(r => r.AdicionarAsync(It.IsAny<Usuario>()))
            .Callback<Usuario>(u => usuarioSalvo = u);

        var request = new RegistrarUsuarioRequest
        {
            Login = " ADMIN ",
            Senha = "senha123",
            Role = RoleUsuario.Operador
        };

        //act
        await _service.RegistrarAsync(request);

        //Assert
        _usuarioRepositoryMock.Verify(
            r => r.AdicionarAsync(It.IsAny<Usuario>()), Times.Once);

        Assert.NotNull(usuarioSalvo);
        Assert.Equal("admin", usuarioSalvo.Login);
        Assert.Equal(RoleUsuario.Operador, usuarioSalvo.Role);
        Assert.NotEqual("senha123", usuarioSalvo.SenhaHash);
        Assert.True(BCrypt.Net.BCrypt.Verify("senha123", usuarioSalvo.SenhaHash));

    }
    [Fact]
    public async Task Login_UsuarioInexistente_LancaCredenciaisInvalidasException()
    {
        //Arrange

        _usuarioRepositoryMock
            .Setup(r => r.ObterPorLoginAsync(It.IsAny<string>()))
            .ReturnsAsync((Usuario?)null);

        var request = new LoginRequest
        {
            Login = "admin",
            Senha = "senha123"
        };

        //ACT + ASSERT
        await Assert.ThrowsAsync<CredenciaisInvalidasException>(() => _service.LoginAsync(request));
    }
    [Fact]

    public async Task Login_SenhaErrada_LancaCredenciaisInvalidasException()
    {
        //Arrange
        var hashCorreto = BCrypt.Net.BCrypt.HashPassword("Senha123@");
        var usuario = new Usuario("admin", hashCorreto, RoleUsuario.Admin);

        _usuarioRepositoryMock
            .Setup(r => r.ObterPorLoginAsync(It.IsAny<string>()))
            .ReturnsAsync(usuario);
        var request = new LoginRequest
        {
            Login = "admin",
            Senha = "Senha@123"
        };

        //Act + Arrange
        await Assert.ThrowsAsync <CredenciaisInvalidasException>(() => _service.LoginAsync(request));
    }
    [Fact]

    public async Task Login_CredenciaisValidas_RetornaTokenComDadosCorretos()
    {
        //Arrange
        var hash = BCrypt.Net.BCrypt.HashPassword("Senha@123");
        var usuario = new Usuario("admin", hash, RoleUsuario.Admin);

        _usuarioRepositoryMock
            .Setup(r => r.ObterPorLoginAsync(It.IsAny<string>()))
            .ReturnsAsync(usuario);
        var request = new LoginRequest
        {
            Login = "admin",
            Senha = "Senha@123"
        };

        //Act 
        var resposta = await _service.LoginAsync(request);

        //Assert
        Assert.False(string.IsNullOrWhiteSpace(resposta.Token));
        Assert.Equal("admin", resposta.Login);
        Assert.Equal("Admin", resposta.Role);
        Assert.True(resposta.ExpiraEm > DateTime.UtcNow);

        var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler()
            .ReadJwtToken(resposta.Token);

        Assert.Equal("TesteIssuer", jwt.Issuer);
        Assert.Contains("TesteAudience", jwt.Audiences);
    }
    [Fact]
    public async Task Login_LoginComEspacosEMaiusculas_NormalizaEAutentica()
    {
        // Arrange
        var hash = BCrypt.Net.BCrypt.HashPassword("Senha@123");
        var usuario = new Usuario("admin", hash, RoleUsuario.Admin);

        _usuarioRepositoryMock
            .Setup(r => r.ObterPorLoginAsync("admin"))
            .ReturnsAsync(usuario);

        var request = new LoginRequest
        {
            Login = "   ADMIN   ",
            Senha = "Senha@123"
        };

        // Act
        var resposta = await _service.LoginAsync(request);

        // Assert
        Assert.Equal("admin", resposta.Login);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Token));
    }
}




