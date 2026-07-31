using Estacionamento.Application.DTos.Requests;
using Estacionamento.Application.DTos.Responses;
using Estacionamento.Application.Interfaces;
using Estacionamento.Domain.Entities;
using Estacionamento.Domain.Exceptions;
using Estacionamento.Domain.Interfaces;
namespace Estacionamento.Application.Services
{
    public class EntradaService : IEntradaService
    {
        private readonly IVagaRepository _vagaRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IVeiculoRepository _veiculoRepository;
        public EntradaService(IVagaRepository vagaRepository, ITicketRepository ticketRepository, IVeiculoRepository veiculoRepository)
        {
            _vagaRepository = vagaRepository;
            _ticketRepository = ticketRepository;
            _veiculoRepository = veiculoRepository;
        }

        public async Task<TicketResponse> RegistrarEntradaAsync(RegistrarEntradaRequest request)
        {
            // 1. Verificar se já existe ticket ativo para a placa
            var ticketAtivo = await _ticketRepository.ObterTicketAtivoPorPlacaAsync(request.Placa);
            if(ticketAtivo is not null)
                throw new VeiculoJaEstacionadoException(request.Placa);

            // 2. Buscar vaga livre compatível com o tipo de veículo
            var vaga = await _vagaRepository.ObterVagaLivreAsync(request.TipoVeiculo);
            if (vaga is null)
                throw new VagaIndisponivelException("Não há vagas disponíveis para este tipo de veículo.");

            // 3. Buscar o veículo pela placa, ou criar um novo se for a primeira vez
            var veiculo = await _veiculoRepository.ObterPorPlacaAsync(request.Placa);
            if (veiculo is null)
            {
                veiculo = new Veiculo(request.Placa, request.TipoVeiculo, request.Modelo, request.Cor);
                await _veiculoRepository.AdicionarAsync(veiculo);
            }

            // 4. Ocupar a vaga e criar o ticket
            vaga.Ocupar();
            var ticket = new Ticket(veiculo.Id, vaga.Id, DateTime.UtcNow);

            await _ticketRepository.AdicionarAsync(ticket);
            await _vagaRepository.AtualizarAsync(vaga);

            // 5. Retornar o DTO de resposta
            return new TicketResponse
            {
                TicketId = ticket.Id,
                Placa = veiculo.Placa,
                VagaCodigo = vaga.Codigo,
                DataHoraEntrada = ticket.DataHoraEntrada,
                Status = ticket.Status.ToString()
            };

        }
    }
}
