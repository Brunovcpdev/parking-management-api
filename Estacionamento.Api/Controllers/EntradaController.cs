using Estacionamento.Application.DTos.Requests;
using Estacionamento.Application.Interfaces;
using Estacionamento.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Estacionamento.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EntradaController : ControllerBase
    {
        private readonly IEntradaService _entradaService;
        
        public EntradaController(IEntradaService entradaService)
        {
            _entradaService = entradaService;
        }
       
        [HttpPost]
        public async Task<IActionResult> RegistarEntrada([FromBody] RegistrarEntradaRequest request)
        {
            try
            {
                var response = await _entradaService.RegistrarEntradaAsync(request);
                return Created(string.Empty, response);
            }
            catch(VeiculoJaEstacionadoException ex)
            {
                return Conflict(new { mensagem = ex.Message });       
            }
            catch(VagaIndisponivelException ex) 
            {
                return Conflict(new { mensagem =ex.Message });
            }
            catch(ArgumentException ex) 
            {
                return BadRequest(new {mensagem = ex.Message});
            }
        }
    }
}
