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
    }
}
