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


        /// <summary>
        /// Realiza login do usuário e gera token JWT.
        /// </summary>
        /// <param name="dto">Email e senha.</param>
        /// <returns>Token JWT para autenticação.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            var token = _authService.Autenticar(dto.Email, dto.Senha);
            if (token == null)
                return Unauthorized(new { mensagem = "Usuário ou senha inválidos." });

            return Ok(new { token });
        }

        /// <summary>
        /// Cadastra um novo usuário no sistema.
        /// </summary>
        /// <param name="dto">Dados de criação do usuário.</param>
        /// <returns>Mensagem de sucesso.</returns>
        [HttpPost("cadastro")]
        public IActionResult Cadastrar([FromBody] Usuario usuario)
        {
            bool sucesso = _authService.Cadastrar(usuario);
            if (!sucesso)
                return BadRequest(new { mensagem = "E-mail já cadastrado." });

            return Ok(new { mensagem = "Usuário cadastrado com sucesso!" });
        }


        /// <summary>
        /// Remove um usuário pelo email.
        /// </summary>
        /// <param name="email">Email do usuário.</param>
        /// <returns>Mensagem de confirmação.</returns>
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
