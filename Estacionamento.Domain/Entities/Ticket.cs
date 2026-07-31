using Estacionamento.Domain.Enums;

namespace Estacionamento.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; private set; }
        public int VagaId { get; private set; }
        public int VeiculoId { get; private set; }
        public DateTime DataHoraEntrada { get; private set; }
        public DateTime? DataHoraSaida { get; private set; } //tem ? porque, enquanto o veículo está no pátio, não existe data de saída ainda. 
        public decimal? ValorCobrado { get; private set; } // mesma lógica: só existe um valor depois que a saída é calculada
        public StatusTicket Status { get; private set; }

        protected Ticket() 
        {
        }
        
        public Ticket (int veiculoId, int vagaId, DateTime dataHoraEntrada)
        {
            VeiculoId = veiculoId;
            VagaId = vagaId;
            DataHoraEntrada = dataHoraEntrada;
            Status = StatusTicket.Ativo;
        }

        public void RegistrarSaida(DateTime dataHoraSaida, decimal valor)
        {
            if (Status != StatusTicket.Ativo)
                throw new InvalidOperationException("Só é possível registrar saída de um ticket ativo.");

            if (dataHoraSaida < DataHoraEntrada)
                throw new ArgumentException("A data de saída não pode ser anterior à data de entrada.", nameof(dataHoraSaida));

            DataHoraSaida = dataHoraSaida;
            ValorCobrado = valor;
            Status = StatusTicket.Finalizado;
        }

        public TimeSpan? TempoPermanencia => //Com expression-bodied property + ternário
            DataHoraSaida.HasValue ? DataHoraSaida.Value - DataHoraEntrada : null;
        //Por DataHoraSaida ser nullable o .HasValue verifica se existe um valor real, se não for nulo (como se fosse if/else)
        //.Value serve para tirar esse valor de dentro do nullable e usar.Fazer isso depois de já ter confirmado com .HasValue que existe algo lá dentro.
    }
}
