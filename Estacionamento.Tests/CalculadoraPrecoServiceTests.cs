using Estacionamento.Application.Services;
using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;

namespace Estacionamento.Tests
{
    public class CalculadoraPrecoServiceTests
    {
        [Fact]
        public void CalcularValor_DeveRetornarZero_QuandoDentroDaTolerancia()
        {
            //Arrange 
            var calculadora = new CalculadoraPrecoService();

            var tabelaPreco = new TabelaPreco(
                tipoVeiculo: TipoVeiculo.Carro,
                toleranciaMinutos: 10,
                fracaoMinutos: 15,
                valorFracao: 5m,
                valorDiaria: 50m);

            var dataEntrada = new DateTime(2026, 8, 5, 10, 0, 0);
            var ticket = new Ticket(veiculoId: 1, vagaId: 1, dataHoraEntrada: dataEntrada);

            var dataSaida = dataEntrada.AddMinutes(8); //dentro da tolerancia de 10 minutos

            //Act
            var valor = calculadora.CalcularValor(ticket, tabelaPreco, dataSaida);

            //Assert
            Assert.Equal(0m, valor);

        }

        [Theory]
        [InlineData(20, 5)] // 20 min: 10 cobráveis / 15 = 0,66 → arredonda para 1 fração → 1 × 5 = 5
        [InlineData(35, 10)] // 35 min: 25 cobráveis / 15 = 1,66 → arredonda para 2 frações → 2 × 5 = 10
        [InlineData(10, 0)] // 10 min: exatamente na tolerância → 0
        [InlineData(180, 50)] // ultrapassa a diária: valor calculado seria 60, mas é limitado a 50
        public void CalcularValor_DeveCalcularCorretamente_QuandoUltrapassaTolerancia(
            int minutosPermanencia, decimal valorEsperado)
        {
            // Arrange
            var calculadora = new CalculadoraPrecoService();

            var tabelaPreco = new TabelaPreco(
                tipoVeiculo: TipoVeiculo.Carro,
                toleranciaMinutos: 10,
                fracaoMinutos: 15,
                valorFracao: 5m,
                valorDiaria: 50m);

            var dataEntrada = new DateTime(2026, 8, 5, 10, 0, 0);
            var ticket = new Ticket(veiculoId: 1, vagaId: 1, dataHoraEntrada: dataEntrada);

            var dataSaida = dataEntrada.AddMinutes(minutosPermanencia);

            // Act
            var valor = calculadora.CalcularValor(ticket, tabelaPreco, dataSaida);

            // Assert
            Assert.Equal(valorEsperado, valor);
        }


    }
}
