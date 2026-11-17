namespace Estacionamento.Application.DTOs.Requests
{
    public class LoginRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Senha {  get; set; }   = string.Empty;
    }
}
