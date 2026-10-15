using Estacionamento.Application.DTos.Requests;
using Estacionamento.Application.DTOs.Request;
using Estacionamento.Application.Interfaces;
using Estacionamento.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Estacionamento.Api.Controllers
{
    [ApiController]
    [Authorize]
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
            catch(TicketNaoEncontradoException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch(VagaNaoEncontradaException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch(VeiculoNaoEncontradoException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch(TabelaPrecoNaoEncontradaException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (TicketJaFinalizadoException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }


        }
    }
}
