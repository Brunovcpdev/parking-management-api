namespace Estacionamento.Application.DTos.Responses
{
    public class TicketResponse
    {
        public int TicketId { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string VagaCodigo { get; set; } = string.Empty;
        public DateTime DataHoraEntrada { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
