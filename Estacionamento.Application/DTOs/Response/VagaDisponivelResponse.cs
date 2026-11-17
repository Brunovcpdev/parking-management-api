namespace Estacionamento.Application.DTOs.Responses
{
    public class VagaDisponivelResponse
    {
        public int TotalVagas {  get; set; }
        public int VagasOcupadas { get; set; }
        public int VagasLivres { get; set; }

    }
}
