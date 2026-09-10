using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;
using Estacionamento.Domain.Interfaces;
using Estacionamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Infrastructure.Repositories
{
    public class VagaRepository : IVagaRepository
    {
        private readonly AppDbContext _context;

        public VagaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Vaga?> ObterVagaLivreAsync(TipoVeiculo tipoVeiculo)
        {
            return await _context.Vagas
                .FirstOrDefaultAsync(v => v.TipoPermitido == tipoVeiculo && v.Status == StatusVaga.Livre);
        }

        public async Task AtualizarAsync(Vaga vaga)
        {
            _context.Vagas.Update(vaga);
            await _context.SaveChangesAsync();
        }

        public async Task<Vaga?> ObterPorIdAsync(int id)
        {
            return await _context.Vagas.FindAsync(id);
        }

        public async Task<List<Vaga>> ObterTodasAsync()
        {
            return await _context.Vagas.ToListAsync();
        }
    }
}
