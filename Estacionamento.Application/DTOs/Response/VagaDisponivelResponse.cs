namespace Estacionamento.Application.DTOs.Response
{
    public class VagaDisponivelResponse
    {
        public int TotalVagas {  get; set; }
        public int VagasOcupadas { get; set; }
        public int VagasLivres { get; set; }

    }
}
