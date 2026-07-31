using Estacionamento.Domain.Entities;

namespace Estacionamento.Domain.Interfaces
{
    public interface IVeiculoRepository
    {
        Task<Veiculo?> ObterPorPlacaAsync(string placa); //Verifica se já existe no banco esse veiculo
        Task AdicionarAsync(Veiculo veiculo); //Só é chamado se for a primeira vez que essa placa aparece no sistema
    }
}
