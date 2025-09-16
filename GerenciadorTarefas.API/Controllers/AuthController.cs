using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorTarefas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AutenticacaoServico _auth;

        public AuthController(AutenticacaoServico auth)
        {
            _auth = auth;
        }

        [HttpPost("registrar")]
        public ActionResult<UsuarioDto> Registrar(RegistroDto dto)
        {
            var usuario = _auth.Registrar(dto);
            return Ok(usuario);
        }

        [HttpPost("login")]
        public ActionResult<UsuarioDto> Login(LoginDto dto)
        {
            var usuario = _auth.Login(dto);
            if (usuario == null) return Unauthorized("Credenciais inválidas");
            return Ok(usuario);
        }
    }
}