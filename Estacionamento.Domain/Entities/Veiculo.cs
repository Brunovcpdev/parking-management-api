using Estacionamento.Domain.Enums;

namespace Estacionamento.Domain.Entities
{
    public class Veiculo
    {
        public int Id { get; private set; }
        public string Placa { get; private set; } = string.Empty;
        public TipoVeiculo Tipo {  get; private set; }
        public string? Modelo { get; private set; }
        public string? Cor {  get; private set; }

        protected Veiculo() 
        { 
        }

        public Veiculo(string placa, TipoVeiculo tipo, string? modelo = null, string? cor = null)
        {
            if(string.IsNullOrWhiteSpace(placa))
                throw new ArgumentException("A placa é obrigatória.", nameof(placa));

            Placa = placa.Trim().ToUpperInvariant();
            Tipo = tipo;
            Modelo = modelo;
            Cor = cor;
        }
    }
}
