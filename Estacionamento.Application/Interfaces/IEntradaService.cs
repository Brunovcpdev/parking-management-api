using Estacionamento.Application.DTos.Requests;
using Estacionamento.Application.DTos.Responses;

namespace Estacionamento.Application.Interfaces
{
    public interface IEntradaService
    {
        Task<TicketResponse> RegistrarEntradaAsync(RegistrarEntradaRequest request);
    }
}
