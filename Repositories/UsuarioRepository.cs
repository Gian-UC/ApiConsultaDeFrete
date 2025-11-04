using EnvioRapidoApi.Models;

namespace EnvioRapidoApi.Repositories
{
    public class UsuarioRepository
    {
        private readonly List<Usuario> _usuarios = new();

        public UsuarioRepository()
        {
            // Usuário padrão só pra teste
            _usuarios.Add(new Usuario
            {
                Nome = "Admin",
                Email = "admin@email.com",
                Senha = "123456"
            });
        }

        public List<Usuario> Listar() => _usuarios;

        public Usuario? BuscarPorEmail(string email)
        {
            return _usuarios.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void Adicionar(Usuario usuario)
        {
            _usuarios.Add(usuario);
        }

        public bool RemoverPorEmail(string email)
        {
            var usuario = _usuarios.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (usuario == null)
                return false;

            _usuarios.Remove(usuario);
            return true;
        }
    }
}
