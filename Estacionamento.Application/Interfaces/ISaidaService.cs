using Estacionamento.Application.DTOs.Responses;
using Estacionamento.Application.DTOs.Requests; 

namespace Estacionamento.Application.Interfaces
{
    public interface ISaidaService 
    {
        Task<ComprovantePagamentoResponse> RegistrarSaidaAsync(RegistrarSaidaRequest request);
    }
}
