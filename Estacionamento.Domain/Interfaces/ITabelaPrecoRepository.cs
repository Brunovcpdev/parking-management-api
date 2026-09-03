using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;

namespace Estacionamento.Domain.Interfaces
{
    public interface ITabelaPrecoRepository
    {
        Task<TabelaPreco?> ObterPorTipoVeiculoAsync(TipoVeiculo tipoVeiculo);
    }
}
