using Estacionamento.Application.DTos.Requests;
using Estacionamento.Application.DTOs.Request;
using Estacionamento.Application.Interfaces;
using Estacionamento.Application.Services;
using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;
using Estacionamento.Domain.Exceptions;
using Estacionamento.Domain.Interfaces;
using Moq;
using Xunit;

namespace Estacionamento.Tests
{
    public class SaidaServiceTests
    {
        [Fact]
        public async Task RegistrarSaidaAsync_DeveLancarExcecao_QuandoTicketNaoExiste()
        {
            //Arrange
            var mockTicketRepository = new Mock<ITicketRepository>();
            var mockVagaRepository = new Mock<IVagaRepository>();
            var mockVeiculoRepository = new Mock<IVeiculoRepository>();
            var mockTabelaPrecoRepository = new Mock<ITabelaPrecoRepository>();
            var mockCalculadoraPrecoService = new Mock<ICalculadoraPrecoService>();

            mockTicketRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Ticket?)null);

            var saidaService = new SaidaService(
                mockTicketRepository.Object,
                mockVagaRepository.Object,
                mockVeiculoRepository.Object,
                mockTabelaPrecoRepository.Object,
                mockCalculadoraPrecoService.Object);

            var request = new RegistrarSaidaRequest { TicketId = 999 };

            // Act
            var act = async () => await saidaService.RegistrarSaidaAsync(request);

            // Assert
            await Assert.ThrowsAsync<TicketNaoEncontradoException>(act);
        }

        [Fact]
        public async Task RegistrarSaidaAsync_DeveLancarExcecao_QuandoTicketJaFinalizado()
        {
            // Arrange
            var ticketFinalizado = new Ticket(veiculoId: 1, vagaId: 1, dataHoraEntrada: DateTime.UtcNow.AddHours(-2));
            ticketFinalizado.RegistrarSaida(DateTime.UtcNow, 10m);

            var mockTicketRepository = new Mock<ITicketRepository>();
            var mockVagaRepository = new Mock<IVagaRepository>();
            var mockVeiculoRepository = new Mock<IVeiculoRepository>();
            var mockTabelaPrecoRepository = new Mock<ITabelaPrecoRepository>();
            var mockCalculadoraPrecoService = new Mock<ICalculadoraPrecoService>();

            mockTicketRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ticketFinalizado);

            var saidaService = new SaidaService(
                mockTicketRepository.Object,
                mockVagaRepository.Object,
                mockVeiculoRepository.Object,
                mockTabelaPrecoRepository.Object,
                mockCalculadoraPrecoService.Object);

            var request = new RegistrarSaidaRequest { TicketId = 1 };

            // Act
            var act = async () => await saidaService.RegistrarSaidaAsync(request);

            // Assert
            await Assert.ThrowsAsync<TicketJaFinalizadoException>(act);
        }

        [Fact]
        public async Task RegistrarSaidaAsync_DeveLancarExcecao_QuandoVagaNaoEncontrada()
        {
            // Arrange
            var ticketAtivo = new Ticket(veiculoId: 1, vagaId: 1, dataHoraEntrada: DateTime.UtcNow.AddHours(-2));
            

            var mockTicketRepository = new Mock<ITicketRepository>();
            var mockVagaRepository = new Mock<IVagaRepository>();
            var mockVeiculoRepository = new Mock<IVeiculoRepository>();
            var mockTabelaPrecoRepository = new Mock<ITabelaPrecoRepository>();
            var mockCalculadoraPrecoService = new Mock<ICalculadoraPrecoService>();

            mockTicketRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ticketAtivo);

            mockVagaRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Vaga?)null);

            var saidaService = new SaidaService(
                mockTicketRepository.Object,
                mockVagaRepository.Object,
                mockVeiculoRepository.Object,
                mockTabelaPrecoRepository.Object,
                mockCalculadoraPrecoService.Object);

            var request = new RegistrarSaidaRequest { TicketId = 1 };

            // Act
            var act = async () => await saidaService.RegistrarSaidaAsync(request);

            // Assert
            await Assert.ThrowsAsync<VagaNaoEncontradaException>(act);
        }

        [Fact]
        public async Task RegistrarSaidaAsync_DeveLancarExcecao_QuandoVeiculoNaoEncontrado()
        {
            // Arrange
            var ticketAtivo = new Ticket(veiculoId: 1, vagaId: 1, dataHoraEntrada: DateTime.UtcNow.AddHours(-2));
            var vagaAtiva = new Vaga("A01", TipoVeiculo.Carro);

            var mockTicketRepository = new Mock<ITicketRepository>();
            var mockVagaRepository = new Mock<IVagaRepository>();
            var mockVeiculoRepository = new Mock<IVeiculoRepository>();
            var mockTabelaPrecoRepository = new Mock<ITabelaPrecoRepository>();
            var mockCalculadoraPrecoService = new Mock<ICalculadoraPrecoService>();

            mockTicketRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ticketAtivo);

            mockVagaRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(vagaAtiva);

            mockVeiculoRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Veiculo?)null);

            var saidaService = new SaidaService(
                mockTicketRepository.Object,
                mockVagaRepository.Object,
                mockVeiculoRepository.Object,
                mockTabelaPrecoRepository.Object,
                mockCalculadoraPrecoService.Object);

            var request = new RegistrarSaidaRequest { TicketId = 1 };

            // Act
            var act = async () => await saidaService.RegistrarSaidaAsync(request);

            // Assert
            await Assert.ThrowsAsync<VeiculoNaoEncontradoException>(act);
        }

        [Fact]
        public async Task RegistrarSaidaAsync_DeveLancarExcecao_QuandoTabelaPrecoNaoEncontrada()
        {
            // Arrange
            var ticketAtivo = new Ticket(veiculoId: 1, vagaId: 1, dataHoraEntrada: DateTime.UtcNow.AddHours(-2));
            var vagaAtiva = new Vaga("A01", TipoVeiculo.Carro);
            var veiculoAtivo = new Veiculo("OLB2F45", TipoVeiculo.Carro);

            var mockTicketRepository = new Mock<ITicketRepository>();
            var mockVagaRepository = new Mock<IVagaRepository>();
            var mockVeiculoRepository = new Mock<IVeiculoRepository>();
            var mockTabelaPrecoRepository = new Mock<ITabelaPrecoRepository>();
            var mockCalculadoraPrecoService = new Mock<ICalculadoraPrecoService>();

            mockTicketRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ticketAtivo);

            mockVagaRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(vagaAtiva);

            mockVeiculoRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(veiculoAtivo);

            mockTabelaPrecoRepository
                .Setup(r => r.ObterPorTipoVeiculoAsync(It.IsAny<TipoVeiculo>()))
                .ReturnsAsync((Domain.Entities.TabelaPreco?)null);

            var saidaService = new SaidaService(
                mockTicketRepository.Object,
                mockVagaRepository.Object,
                mockVeiculoRepository.Object,
                mockTabelaPrecoRepository.Object,
                mockCalculadoraPrecoService.Object);

            var request = new RegistrarSaidaRequest { TicketId = 1 };

            // Act
            var act = async () => await saidaService.RegistrarSaidaAsync(request);

            // Assert
            await Assert.ThrowsAsync<TabelaPrecoNaoEncontradaException>(act);
        }

        [Fact]
        public async Task RegistrarSaidaAsync_DeveRetornarComprovante_QuandoTudoValido()
        {
            // Arrange
            var dataEntrada = DateTime.UtcNow.AddHours(-2);
            var ticketAtivo = new Ticket(veiculoId: 1, vagaId: 1, dataHoraEntrada: dataEntrada);
            var vagaAtiva = new Vaga("A01", TipoVeiculo.Carro);
            var veiculoAtivo = new Veiculo("OLB2F45", TipoVeiculo.Carro);
            var tabelaPreco = new TabelaPreco(
                tipoVeiculo: TipoVeiculo.Carro,
                toleranciaMinutos: 10,
                fracaoMinutos: 15,
                valorFracao: 5m,
                valorDiaria: 50m);

            var valorEsperado = 20m; // valor que vamos "forçar" a calculadora a devolver

            var mockTicketRepository = new Mock<ITicketRepository>();
            var mockVagaRepository = new Mock<IVagaRepository>();
            var mockVeiculoRepository = new Mock<IVeiculoRepository>();
            var mockTabelaPrecoRepository = new Mock<ITabelaPrecoRepository>();
            var mockCalculadoraPrecoService = new Mock<ICalculadoraPrecoService>();

            mockTicketRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ticketAtivo);

            mockVagaRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(vagaAtiva);

            mockVeiculoRepository
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(veiculoAtivo);

            mockTabelaPrecoRepository
                .Setup(r => r.ObterPorTipoVeiculoAsync(It.IsAny<TipoVeiculo>()))
                .ReturnsAsync(tabelaPreco);

            mockCalculadoraPrecoService
                .Setup(c => c.CalcularValor(ticketAtivo, tabelaPreco, It.IsAny<DateTime>()))
                .Returns(valorEsperado);

            var saidaService = new SaidaService(
                mockTicketRepository.Object,
                mockVagaRepository.Object,
                mockVeiculoRepository.Object,
                mockTabelaPrecoRepository.Object,
                mockCalculadoraPrecoService.Object);

            var request = new RegistrarSaidaRequest { TicketId = 1 };

            // Act
            var response = await saidaService.RegistrarSaidaAsync(request);

            // Assert
            Assert.Equal(ticketAtivo.Id, response.TicketId);
            Assert.Equal(veiculoAtivo.Placa, response.Placa);
            Assert.Equal(valorEsperado, response.ValorCobrado);
            Assert.Equal(StatusTicket.Finalizado, ticketAtivo.Status);
            Assert.Equal(StatusVaga.Livre, vagaAtiva.Status);
        }
    }
}
