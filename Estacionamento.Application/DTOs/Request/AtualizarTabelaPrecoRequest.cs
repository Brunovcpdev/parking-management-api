namespace Estacionamento.Application.DTOs.Requests
{
    public class AtualizarTabelaPrecoRequest
    {
        public int ToleranciaMinutos { get; set; }
        public int FracaoMinutos { get; set; }
        public decimal ValorFracao { get; set; }
        public decimal ValorDiaria { get; set; }
    }
}
