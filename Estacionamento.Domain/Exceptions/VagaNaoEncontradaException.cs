namespace Estacionamento.Domain.Exceptions
{
    public class VagaNaoEncontradaException : Exception
    {
        public VagaNaoEncontradaException(int vagaId)
            : base ($"Vaga {vagaId} não encontrada.")
        {
        } 
    }
}
