namespace Estacionamento.Domain.Exceptions
{
    public class VeiculoNaoEncontradoException : Exception
    {
        public VeiculoNaoEncontradoException(int veiculoId)
            : base($"Veiculo {veiculoId} não encontrado.")
        {
        }
    }
}
