namespace Estacionamento.Domain.Exceptions
{
    public class TicketNaoEncontradoException : Exception
    {
        public TicketNaoEncontradoException(int ticketId)
            : base ($"Ticket {ticketId} não encontrado.")
        {
        }
    }
}
