using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;

namespace Estacionamento.Domain.Interfaces
{
    public interface IVagaRepository
    {
        Task<Vaga?> ObterVagaLivreAsync(TipoVeiculo tipoVeiculo);
        Task<Vaga?> ObterPorIdAsync(int id);
        Task AtualizarAsync(Vaga vaga);
    }
}
