using Estacionamento.Application.DTos.Requests;
using Estacionamento.Application.DTos.Responses;
using Estacionamento.Application.DTOs.Request;
using Estacionamento.Application.DTOs.Response;
using Estacionamento.Application.Interfaces;
using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Enums;
using Estacionamento.Domain.Interfaces;


namespace Estacionamento.Application.Services
{
    public class SaidaService : ISaidaService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IVagaRepository _vagaRepository;
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly ITabelaPrecoRepository _tabelaPrecoRepository;
        private readonly ICalculadoraPrecoService _calculadoraPrecoService;

        public SaidaService(
            ITicketRepository ticketRepository,
            IVagaRepository vagaRepository,
            IVeiculoRepository veiculoRepository,
            ITabelaPrecoRepository tabelaPrecoRepository,
            ICalculadoraPrecoService calculadoraPrecoService)
        {
            _ticketRepository = ticketRepository;
            _vagaRepository = vagaRepository;
            _veiculoRepository = veiculoRepository;
            _tabelaPrecoRepository = tabelaPrecoRepository;
            _calculadoraPrecoService = calculadoraPrecoService;
        }

        public async Task<ComprovantePagamentoResponse> RegistrarSaidaAsync(RegistrarSaidaRequest request)
        {
            // 1. Buscar o ticket e validar se existe e está ativo
            var ticket = await _ticketRepository.ObterPorIdAsync(request.TicketId);
            if (ticket is null)
                throw new InvalidOperationException($"Ticket {request.TicketId} não encontrado");

            if (ticket.Status != StatusTicket.Ativo)
                throw new InvalidOperationException($"Ticket {request.TicketId} já foi finalizado ou cancelado.");

            // 2. Buscar a vaga e o veículo relacionados ao ticket
            var vaga = await _vagaRepository.ObterPorIdAsync(ticket.VagaId);
            if (vaga is null)
                throw new InvalidOperationException($"Vaga {ticket.VagaId} não encontrada.");
            
            var veiculo = await _veiculoRepository.ObterPorIdAsync(ticket.VeiculoId);
            if (veiculo is null)
                throw new InvalidOperationException($"Veículo {ticket.VeiculoId} não encontrado.");

            // 3. Buscar a tabela de preço correspondente ao tipo do veículo
            var tabelaPreco = await _tabelaPrecoRepository.ObterPorTipoVeiculoAsync(veiculo.Tipo);
            if (tabelaPreco is null)
                throw new InvalidOperationException($"Não há tabela de preço cadastrada para o tipo {veiculo.Tipo}.");

            // 4. Calcular o valor a pagar
            var dataHoraSaida = DateTime.UtcNow;
            var valorCobrado = _calculadoraPrecoService.CalcularValor(ticket, tabelaPreco, dataHoraSaida);

            // 5. Registrar a saída no ticket e liberar a vaga
            ticket.RegistrarSaida(dataHoraSaida, valorCobrado);
            vaga.Liberar();

            await _ticketRepository.AtualizarAsync(ticket);
            await _vagaRepository.AtualizarAsync(vaga);

            // 6. Retornar o comprovante de pagamento
            return new ComprovantePagamentoResponse
            {
                TicketId = ticket.Id,
                Placa = veiculo.Placa,
                DataHoraEntrada = ticket.DataHoraEntrada,
                DataHoraSaida = dataHoraSaida,
                TempoPermanencia = ticket.TempoPermanencia!.Value,
                ValorCobrado = valorCobrado
            };
        }
    }
}
