using Estacionamento.Application.DTOs.Response;
using Estacionamento.Application.Interfaces;
using Estacionamento.Domain.Interfaces;
using Estacionamento.Domain.Enums;

namespace Estacionamento.Application.Services
{
    public class VagaConsultaService : IVagaConsultaService
    {
        private readonly IVagaRepository _vagaRepository;

        public VagaConsultaService(IVagaRepository vagaRepository)
        {
            _vagaRepository = vagaRepository;
        }

        public async Task<VagaDisponivelResponse> ObterResumoVagasAsync()
        {
            var vagas = await _vagaRepository.ObterTodasAsync();

            var total = vagas.Count();

            var livres = vagas.Count(v => v.Status == StatusVaga.Livre);

            var ocupadas = vagas.Count(v => v.Status == StatusVaga.Ocupada);

            return new VagaDisponivelResponse
            {
                TotalVagas = total,
                VagasLivres = livres,
                VagasOcupadas = ocupadas
            };


        }
    }
}
