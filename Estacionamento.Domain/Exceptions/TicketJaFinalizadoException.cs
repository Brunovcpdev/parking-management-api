namespace Estacionamento.Domain.Exceptions
{
    public class TicketJaFinalizadoException : Exception
    {
        public TicketJaFinalizadoException(int ticketId)
            : base($"Ticket {ticketId} já foi finalizado.")
        {
        }
    }
}
