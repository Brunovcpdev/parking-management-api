using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;

namespace Estacionamento.Domain.Interfaces
{
    public interface IVagaRepository
    {
        Task<Vaga?> ObterVagaLivreAsync(TipoVeiculo tipoVeiculo);
        Task AtualizarAsync(Vaga vaga);
    }
}
