using Estacionamento.Domain.Enums;
using Estacionamento.Domain.Exceptions;
namespace Estacionamento.Domain.Entities
{
    public class Vaga
    {
        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public TipoVeiculo TipoPermitido { get; private set; }
        public StatusVaga Status {  get; private set; }

        protected Vaga()
        {
        }

        public Vaga (string codigo, TipoVeiculo tipoPermitido)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("O código da vaga é obrigatório.", nameof(codigo));

            Codigo = codigo.Trim().ToUpperInvariant();
            TipoPermitido = tipoPermitido;
            Status = StatusVaga.Livre;
        }

        public void Ocupar()
        {
            if (Status == StatusVaga.Bloqueada)
                throw new VagaIndisponivelException($"A vaga {Codigo} está bloqueada e não pode receber veiculos.");
            if (Status == StatusVaga.Ocupada)
                throw new VagaIndisponivelException($"A vaga {Codigo} está ocupada");

            Status = StatusVaga.Ocupada;
        }

        public void Liberar()
        {
            Status = StatusVaga.Livre;
        }

        public void Bloquear()
        {
            Status = StatusVaga.Ocupada;
        }
    }
}
