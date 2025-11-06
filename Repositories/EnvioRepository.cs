using EnvioRapidoApi.Data;
using EnvioRapidoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EnvioRapidoApi.Repositories
{
    public class EnvioRepository
    {
        private readonly AppDbContext _context;

        public EnvioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Envio> SalvarAsync(Envio envio)
        {
            _context.Envios.Add(envio);
            await _context.SaveChangesAsync();
            return envio;
        }

        public async Task<Envio?> BuscarPorIdAsync(int id)
        {
            return await _context.Envios.FindAsync(id);
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var envio = await _context.Envios.FindAsync(id);
            if (envio == null)
                return false;

            _context.Envios.Remove(envio);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteEnvioIgualAsync(string origemCep, string destinoCep, decimal peso)
        {
            return await _context.Envios
                .AnyAsync(e =>
                    e.OrigemCep == origemCep &&
                    e.DestinoCep == destinoCep &&
                    e.Peso == peso
                );
        }
    }
}
