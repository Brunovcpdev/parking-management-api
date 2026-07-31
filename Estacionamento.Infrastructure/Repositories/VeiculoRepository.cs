using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Interfaces;
using Estacionamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Infrastructure.Repositories
{
    public class VeiculoRepository : IVeiculoRepository
    {
        private readonly AppDbContext _context;

        public VeiculoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Veiculo?> ObterPorPlacaAsync(string placa)
        {
            return await _context.Veiculos
                .FirstOrDefaultAsync(v => v.Placa == placa);
        }
        public async Task AdicionarAsync(Veiculo veiculo)
        {
            await _context.Veiculos.AddAsync(veiculo);
            await _context.SaveChangesAsync();
        }
    }
}
