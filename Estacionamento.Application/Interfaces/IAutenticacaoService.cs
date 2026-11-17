using Estacionamento.Application.DTOs.Requests;
using Estacionamento.Application.DTOs.Responses;

namespace Estacionamento.Application.Interfaces
{
    public interface IAutenticacaoService
    {
        Task RegistrarAsync(RegistrarUsuarioRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
