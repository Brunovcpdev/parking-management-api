using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;
namespace Estacionamento.Domain.Interfaces

{
    public interface ITicketRepository
    {
        Task<Ticket?> ObterTicketAtivoPorPlacaAsync(string placa);
        Task AdicionarAsync(Ticket ticket);
        Task<Ticket?> ObterPorIdAsync(int id);
        Task AtualizarAsync(Ticket ticket);
        Task<bool> ExisteTicketAtivoParaTipoVeiculoAsync(TipoVeiculo tipoVeiculo);

    }
}
