using Estacionamento.Domain.Enums;

namespace Estacionamento.Application.DTos.Requests
{
    public class RegistrarEntradaRequest
    {
        public string Placa {  get; set; } = string.Empty;
        public TipoVeiculo TipoVeiculo { get; set; }
        public string? Cor {  get; set; }
        public string? Modelo { get; set; }
    }
}
