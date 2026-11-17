using Estacionamento.Domain.Enums;

namespace Estacionamento.Application.DTOs.Responses
{
    public class TabelaPrecoResponse
    {
        public int TabelaId { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
        public int ToleranciaMinutos { get; set; }
        public int FracaoMinutos { get; set; }
        public decimal ValorFracao { get; set; }
        public decimal ValorDiaria { get; set; }
    }
}
