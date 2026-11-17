using Estacionamento.Domain.Enums;

namespace Estacionamento.Domain.Exceptions
{
    public class TabelaPrecoJaExisteException : Exception
    {
        public TabelaPrecoJaExisteException(TipoVeiculo tipoVeiculo) : 
            base ($"Já existe uma tabela com esse tipo de veículo '{tipoVeiculo}'.")
        {
        }
             
    }

}
