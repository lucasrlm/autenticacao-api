using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutenticacaoAPI.Services;
using AutenticacaoAPI.Entities;

namespace AutenticacaoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private IUsuarioServico _UsuarioServico;

        public UsuariosController(IUsuarioServico usuarioServico)
        {
            _UsuarioServico = usuarioServico;
        }

        [AllowAnonymous]
        [HttpPost("autenticacao")]
        public IActionResult Autenticar([FromBody]Usuario parametrosUsuario)
        {
            var usuario = _UsuarioServico.Autenticar(parametrosUsuario.UsuarioNome, parametrosUsuario.Senha);

            if (usuario == null)
                return Unauthorized(new { message = "Credenciais inválidas." });

            return Ok(usuario);
        }
    }
}