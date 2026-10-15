using Estacionamento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Estacionamento.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class VagaController : ControllerBase
    {
        private readonly IVagaConsultaService _vagaConsultaService;

        public VagaController(IVagaConsultaService vagaConsultaService)
        {
            _vagaConsultaService = vagaConsultaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterResumoVagas()
        {
            var response = await _vagaConsultaService.ObterResumoVagasAsync();
            return Ok(response);
        }
    }
}
