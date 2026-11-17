using Estacionamento.Domain.Enums;

namespace Estacionamento.Domain.Exceptions
{
    public class TabelaPrecoEmUsoException : Exception
    {
        public TabelaPrecoEmUsoException(TipoVeiculo tipoVeiculo) :
            base($"Não é possível remover a tabela de preço do tipo '{tipoVeiculo}' enquanto houver tickets ativos para esse tipo de veículo.")
        {
        }  
    }
}
