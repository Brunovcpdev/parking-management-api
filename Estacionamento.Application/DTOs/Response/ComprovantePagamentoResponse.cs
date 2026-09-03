namespace Estacionamento.Application.DTOs.Response
{
    public class ComprovantePagamentoResponse
    {
        public int TicketId { get; set; }
        public string Placa {  get; set; } = string.Empty;
        public DateTime DataHoraEntrada {  get; set; }
        public DateTime DataHoraSaida { get; set; }
        public TimeSpan TempoPermanencia {  get; set; }
        public decimal ValorCobrado {  get; set; }

    }
}
