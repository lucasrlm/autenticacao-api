using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AutenticacaoAPI.Entities;
using AutenticacaoAPI.Helpers;

namespace AutenticacaoAPI.Services
{
    public interface IUsuarioServico
    {
        Usuario Autenticar(string username, string password);
    }

    public class UsuarioServico : IUsuarioServico
    {
        private List<Usuario> ListaUsuario = new List<Usuario>
        {
            new Usuario { Id = 1, UsuarioNome = "usuario1", Senha = "senha1" }
        };

        private readonly AppSettings AppSettings;

        public UsuarioServico(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        public Usuario Autenticar(string usuarioNome, string senha)
        {
            var usuario = ListaUsuario.SingleOrDefault(x => x.UsuarioNome == usuarioNome && x.Senha == senha);

            // return null if user not found
            if (usuario == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            usuario.Token = tokenHandler.WriteToken(token);

            usuario.Senha = null;

            return usuario;
        }
    }
}