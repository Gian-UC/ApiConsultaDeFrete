using Microsoft.AspNetCore.Mvc;
using EnvioRapidoApi.DTOs;
using EnvioRapidoApi.Models;
using EnvioRapidoApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace EnvioRapidoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AuthService _authService;

        public UsuariosController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            var token = _authService.Autenticar(dto.Email, dto.Senha);
            if (token == null)
                return Unauthorized(new { mensagem = "Usuário ou senha inválidos." });

            return Ok(new { token });
        }

        [HttpPost("cadastro")]
        public IActionResult Cadastrar([FromBody] Usuario usuario)
        {
            bool sucesso = _authService.Cadastrar(usuario);
            if (!sucesso)
                return BadRequest(new { mensagem = "E-mail já cadastrado." });

            return Ok(new { mensagem = "Usuário cadastrado com sucesso!" });
        }

        [Authorize]
        [HttpDelete("{email}")]
        public IActionResult Excluir(string email)
        {
            bool removido = _authService.ExcluirUsuario(email);

            if (!removido)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            return Ok(new { mensagem = "Usuário excluído com sucesso!" });
        }
    }
}
