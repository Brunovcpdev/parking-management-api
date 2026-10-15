namespace Estacionamento.Domain.Exceptions
{
    public class CredenciaisInvalidasException : Exception
    {
        public CredenciaisInvalidasException() 
            :base ($"login ou senha inválidos.")
        {

        }
    }
    
   
}
