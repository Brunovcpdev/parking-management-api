
using Azure.Core;
using Estacionamento.Application.DTOs.Request;
using Estacionamento.Application.Interfaces;
using Estacionamento.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Estacionamento.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AutenticacaoController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioRequest request)
        {
            try
            {
                await _autenticacaoService.RegistrarAsync(request);
                return StatusCode(StatusCodes.Status201Created);

            }
            catch(UsuarioJaExisteException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
            catch(ArgumentException ex)
            {
                return BadRequest(new {mensagem = ex.Message});
            }     
           
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _autenticacaoService.LoginAsync(request);
                return Ok(response);

            }
            catch(CredenciaisInvalidasException ex)
            {
                return Unauthorized(new { mensagem = ex.Message });
            }

        }
    }
}
