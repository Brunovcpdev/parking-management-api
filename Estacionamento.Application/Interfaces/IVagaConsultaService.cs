using Estacionamento.Application.DTOs.Response;

namespace Estacionamento.Application.Interfaces
{
    public interface IVagaConsultaService
    {
        Task<VagaDisponivelResponse> ObterResumoVagasAsync();
    }
}
