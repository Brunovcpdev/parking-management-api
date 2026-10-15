using Estacionamento.Domain.Enums;

namespace Estacionamento.Application.DTOs.Request
{
    public class RegistrarUsuarioRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Senha {  get; set; } = string.Empty;
        public RoleUsuario Role {  get; set; }
    }
}
