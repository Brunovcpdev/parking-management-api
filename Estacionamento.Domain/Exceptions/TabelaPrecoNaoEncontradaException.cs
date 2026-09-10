using Estacionamento.Domain.Enums;

namespace Estacionamento.Domain.Exceptions
{
    public class TabelaPrecoNaoEncontradaException : Exception
    {
        public TabelaPrecoNaoEncontradaException(TipoVeiculo tipoVeiculo):
            base ($"Tabela de preço para o tipo {tipoVeiculo} não encontrada.")
        {
        }
    }
}
