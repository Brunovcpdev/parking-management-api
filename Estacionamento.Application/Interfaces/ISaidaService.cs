using Estacionamento.Application.DTOs.Response;
using Estacionamento.Application.DTOs.Request; 

namespace Estacionamento.Application.Interfaces
{
    public interface ISaidaService 
    {
        Task<ComprovantePagamentoResponse> RegistrarSaidaAsync(RegistrarSaidaRequest request);
    }
}
