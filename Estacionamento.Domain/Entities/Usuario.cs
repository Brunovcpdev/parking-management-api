using Estacionamento.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Estacionamento.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public RoleUsuario Role {  get; set; }

        protected Usuario() 
        {
        }

        public Usuario (string login, string senhaHash, RoleUsuario role)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("O login é obrigatório", nameof(login));

            if (string.IsNullOrWhiteSpace(senhaHash))
                throw new ArgumentException("A senha é obrigatória", nameof(senhaHash));

            Login = login.Trim().ToLowerInvariant();
            SenhaHash = senhaHash;
            Role = role;
        }
    }
}
