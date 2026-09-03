using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;
using Estacionamento.Domain.Interfaces;
using Estacionamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket?> ObterTicketAtivoPorPlacaAsync (string placa)
        {
            return await _context.Tickets
                .Join(_context.Veiculos,
                    ticket => ticket.VeiculoId,
                    veiculo => veiculo.Id,
                    (ticket, veiculo) => new { ticket, veiculo })
                .Where(tv => tv.veiculo.Placa == placa && tv.ticket.Status == StatusTicket.Ativo)
                .Select(tv => tv.ticket)
                .FirstOrDefaultAsync();
        }

        public async Task AdicionarAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task <Ticket?> ObterPorIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task AtualizarAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
