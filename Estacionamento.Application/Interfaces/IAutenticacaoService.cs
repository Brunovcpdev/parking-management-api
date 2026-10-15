using Estacionamento.Application.DTOs.Request;
using Estacionamento.Application.DTOs.Response;

namespace Estacionamento.Application.Interfaces
{
    public interface IAutenticacaoService
    {
        Task RegistrarAsync(RegistrarUsuarioRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
