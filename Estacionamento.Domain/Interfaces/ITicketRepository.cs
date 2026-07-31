using Estacionamento.Domain.Entities;
namespace Estacionamento.Domain.Interfaces

{
    public interface ITicketRepository
    {
        Task<Ticket?> ObterTicketAtivoPorPlacaAsync(string placa);
        Task AdicionarAsync(Ticket ticket);
        Task<Ticket?> ObterPorIdAsync(int id);
        Task AtualizarAsync(Ticket ticket);
    }
}
