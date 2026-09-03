using Estacionamento.Application.Interfaces;
using Estacionamento.Domain.Entities;

namespace Estacionamento.Application.Services
{
    public class CalculadoraPrecoService : ICalculadoraPrecoService
    {
        public decimal CalcularValor(Ticket ticket, TabelaPreco tabelaPreco, DateTime dataHoraSaida)
        {
            var tempoPermanencia = dataHoraSaida - ticket.DataHoraEntrada;
            var minutosTotais = tempoPermanencia.TotalMinutes;

            // Dentro da tolerância: não há cobrança
            if (minutosTotais <= tabelaPreco.ToleranciaMinutos)
                return 0m;

            // Calcula quantas frações (arredondadas para cima) se passaram
            var minutosCobraveis = minutosTotais - tabelaPreco.ToleranciaMinutos;
            var quantidadeFracoes = Math.Ceiling(minutosCobraveis / tabelaPreco.FracaoMinutos);

            var valorCalculado = (decimal)quantidadeFracoes * tabelaPreco.ValorFracao;

            // Se ultrapassar a diária, cobra-se o valor da diária
            return Math.Min (valorCalculado, tabelaPreco.ValorDiaria); //Math.Min é como se fosse um if/else, a mesma ideia numa linha só, de forma mais direta
        }
    }
}
