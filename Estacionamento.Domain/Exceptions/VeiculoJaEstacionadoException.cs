namespace Estacionamento.Domain.Exceptions
{
    public class VeiculoJaEstacionadoException : Exception
    {
        public VeiculoJaEstacionadoException(string placa)
            : base ($"O veiculo de placa {placa} já possui uma entrada ativa no estacionamento")
        {

        }
    }
}
