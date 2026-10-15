namespace Estacionamento.Domain.Exceptions
{
    public class UsuarioJaExisteException : Exception
    {
        public UsuarioJaExisteException(string login)
            : base ($"Já existe um usuário com o login '{login}'.")
        {

        }
        

      
    }
}
