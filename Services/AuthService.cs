using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EnvioRapidoApi.Models;
using EnvioRapidoApi.Repositories;

namespace EnvioRapidoApi.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly UsuarioRepository _repo;

        public AuthService(IConfiguration config, UsuarioRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        public string? Autenticar(string email, string senha)
        {
            var usuario = _repo.BuscarPorEmail(email);

            if (usuario == null || usuario.Senha != senha)
                return null;

            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool Cadastrar(Usuario usuario)
        {
            var existente = _repo.BuscarPorEmail(usuario.Email);
            if (existente != null)
                return false;

            _repo.Adicionar(usuario);
            return true;
        }

        public bool ExcluirUsuario(string email)
        {
            return _repo.RemoverPorEmail(email);
        }
    }
}
