using Estacionamento.Domain.Enums;
namespace Estacionamento.Domain.Entities
{
    public class TabelaPreco
    {
        public int Id { get; private set; }
        public TipoVeiculo TipoVeiculo { get; private set; }
        public int ToleranciaMinutos { get; private set; }
        public int FracaoMinutos { get; private set; }
        public decimal ValorFracao { get; private set; }
        public decimal ValorDiaria { get; private set; }

        protected TabelaPreco()
        {
        }

        public TabelaPreco(TipoVeiculo tipoVeiculo, int toleranciaMinutos, int fracaoMinutos, decimal valorFracao, decimal valorDiaria)
        {
            if (toleranciaMinutos < 0)
                throw new ArgumentException("A tolerância não pode ser negativa.", nameof(toleranciaMinutos));

            if (fracaoMinutos <= 0)
                throw new ArgumentException("A fração de cobrança deve ser maior que zero.", nameof(fracaoMinutos));

            if (valorFracao < 0 || valorDiaria < 0)
                throw new ArgumentException("Os valores de cobrança não podem ser negativos.");

            TipoVeiculo = tipoVeiculo;
            ToleranciaMinutos = toleranciaMinutos;
            FracaoMinutos = fracaoMinutos; 
            ValorFracao = valorFracao;
            ValorDiaria = valorDiaria;
        }
    }
}
