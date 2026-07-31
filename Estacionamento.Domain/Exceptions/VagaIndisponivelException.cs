namespace Estacionamento.Domain.Exceptions
{
    public class VagaIndisponivelException : Exception
    {
        public VagaIndisponivelException(string mensagem) : base(mensagem)
        {

        }
    }
}
