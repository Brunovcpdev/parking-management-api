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
            if (!Enum.IsDefined(tipoVeiculo))
                throw new ArgumentException("O tipo de veículo informado é inválido.", nameof(tipoVeiculo));

            TipoVeiculo = tipoVeiculo;
            Atualizar(toleranciaMinutos, fracaoMinutos, valorFracao, valorDiaria);

        }

        public void Atualizar (int toleranciaMinutos, int fracaoMinutos, decimal valorFracao, decimal valorDiaria)
        {
            if (toleranciaMinutos < 0)
                throw new ArgumentException("A tolerância não pode ser negativa.", nameof(toleranciaMinutos));

            if (fracaoMinutos <= 0)
                throw new ArgumentException("A fração de cobrança deve ser maior que zero.", nameof(fracaoMinutos));

            if (valorFracao < 0 || valorDiaria < 0)
                throw new ArgumentException("Os valores de cobrança não podem ser negativos.");

            if (valorDiaria < valorFracao)
                throw new ArgumentException("O valor da diária não pode ser menor que o valor de uma fração.", nameof(valorDiaria));
    
            ToleranciaMinutos = toleranciaMinutos;
            FracaoMinutos = fracaoMinutos;
            ValorFracao = valorFracao;
            ValorDiaria = valorDiaria;
        }
    }
}
