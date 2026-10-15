using Estacionamento.Domain.Enums;

namespace Estacionamento.Application.DTOs.Response
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public DateTime ExpiraEm { get; set; }
    }
}
