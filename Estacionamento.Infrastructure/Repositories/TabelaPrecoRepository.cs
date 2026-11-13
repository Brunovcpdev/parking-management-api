using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;
using Estacionamento.Domain.Interfaces;
using Estacionamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Infrastructure.Repositories
{
    public class TabelaPrecoRepository : ITabelaPrecoRepository
    {
        private readonly AppDbContext _context;

        public TabelaPrecoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TabelaPreco?> ObterPorTipoVeiculoAsync(TipoVeiculo tipoVeiculo)
        {
            return await _context.TabelaPrecos
                .FirstOrDefaultAsync(t => t.TipoVeiculo == tipoVeiculo);
        }
        public async Task<TabelaPreco?> ObterPorIdAsync (int id)
        {
            return await _context.TabelaPrecos
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<List<TabelaPreco>> ListarAsync()
        {
            return await _context.TabelaPrecos
                .AsNoTracking()
                .OrderBy(t => t.TipoVeiculo)
                .ToListAsync();
        }

        public async Task AdicionarAsync(TabelaPreco tabela)
        {
            await _context.TabelaPrecos.AddAsync(tabela);
            await _context.SaveChangesAsync();
        }
        
        public async Task AtualizarAsync (TabelaPreco tabela)
        {
            _context.TabelaPrecos.Update(tabela);
            await _context.SaveChangesAsync();
        }
        
        public async Task RemoverAsync (TabelaPreco tabela)
        {
            _context.TabelaPrecos.Remove(tabela);
            await _context.SaveChangesAsync();
        }

    }
}
