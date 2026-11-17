using Estacionamento.Application.DTOs.Responses;

namespace Estacionamento.Application.Interfaces
{
    public interface IVagaConsultaService
    {
        Task<VagaDisponivelResponse> ObterResumoVagasAsync();
    }
}
