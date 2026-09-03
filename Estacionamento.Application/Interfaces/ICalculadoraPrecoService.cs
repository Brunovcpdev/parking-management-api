using Estacionamento.Domain.Entities;

namespace Estacionamento.Application.Interfaces
{
    public interface ICalculadoraPrecoService
    {
        decimal CalcularValor(Ticket ticket, TabelaPreco tabelaPreco, DateTime dataHoraSaida);
    }
}
