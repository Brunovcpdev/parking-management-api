using Estacionamento.Domain.Entities;

namespace Estacionamento.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorLoginAsync(string login);
        Task AdicionarAsync(Usuario login);
    }
}
