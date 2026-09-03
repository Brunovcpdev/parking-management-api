using Estacionamento.Application.DTos.Requests;
using Estacionamento.Application.DTOs.Request;
using Estacionamento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaidaController : ControllerBase
    {
        private readonly ISaidaService _saidaService;

        public SaidaController (ISaidaService saidaService)
        {
            _saidaService = saidaService;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarSaida([FromBody]RegistrarSaidaRequest request)
        {
            try
            {
                var response = await _saidaService.RegistrarSaidaAsync(request);
                    return Ok(response);
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
