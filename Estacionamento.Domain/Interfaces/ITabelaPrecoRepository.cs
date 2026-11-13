using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;

namespace Estacionamento.Domain.Interfaces
{
    public interface ITabelaPrecoRepository
    {
        Task<TabelaPreco?> ObterPorTipoVeiculoAsync(TipoVeiculo tipoVeiculo);
        Task<TabelaPreco?> ObterPorIdAsync(int  id);
        Task<List<TabelaPreco>> ListarAsync();
        Task AdicionarAsync(TabelaPreco tabela);
        Task AtualizarAsync(TabelaPreco tabela);
        Task RemoverAsync(TabelaPreco tabela);
    }
}
