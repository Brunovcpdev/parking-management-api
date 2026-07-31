using Estacionamento.Application.DTos.Requests;
using Estacionamento.Application.Services;
using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;
using Estacionamento.Domain.Exceptions;
using Estacionamento.Domain.Interfaces;
using Moq;
using Xunit;
namespace Estacionamento.Tests
{
    public class EntradaServiceTests
    {
        [Fact]
        public async Task RegistrarEntradaAsync_DeveLancarExcecao_QuandoJaExisteTicketAtivoParaPlaca()
        {
            // Arrange
            var placa = "ABC1234";

            var ticketAtivo = new Ticket(veiculoId: 1, vagaId: 1, dataHoraEntrada: DateTime.UtcNow);

            var mockVagaRepository = new Mock<IVagaRepository>();
            var mockTicketRepository = new Mock<ITicketRepository>();
            var mockVeiculoRepository = new Mock<IVeiculoRepository>();

            mockTicketRepository
                .Setup(r => r.ObterTicketAtivoPorPlacaAsync(placa))
                .ReturnsAsync(ticketAtivo);

            var entradaService = new EntradaService(mockVagaRepository.Object,mockTicketRepository.Object, mockVeiculoRepository.Object);

            var request = new RegistrarEntradaRequest
            {
                Placa = placa,
                TipoVeiculo = TipoVeiculo.Carro
            };

            //Act
            var act = async () => await entradaService.RegistrarEntradaAsync(request);

            //Assert
            await Assert.ThrowsAsync<VeiculoJaEstacionadoException>(act);
        }

        [Fact]
        public async Task RegistrarEntradaAsync_DeveLancarExcecao_QuandoNaoHaVagaDisponivel()
        {
            //Arrange
            var placa = "XYZ9876";

            var mockVagaRepository = new Mock<IVagaRepository>();
            var mockTicketRepository = new Mock<ITicketRepository>();
            var mockVeiculoRepository = new Mock<IVeiculoRepository>();

            mockTicketRepository
                .Setup(r => r.ObterTicketAtivoPorPlacaAsync(placa))
                .ReturnsAsync((Ticket?)null);

            mockVagaRepository
                .Setup(r => r.ObterVagaLivreAsync(TipoVeiculo.Carro))
                .ReturnsAsync((Vaga?)null);

            var entradaService = new EntradaService(mockVagaRepository.Object, mockTicketRepository.Object, mockVeiculoRepository.Object);

            var request = new RegistrarEntradaRequest
            {
                Placa = placa,
                TipoVeiculo = TipoVeiculo.Carro
            };

            //Act
            var act = async () => await entradaService.RegistrarEntradaAsync(request);

            //Assert
            await Assert.ThrowsAsync<VagaIndisponivelException>(act);


        }
    }
}
