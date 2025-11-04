using Microsoft.EntityFrameworkCore;
using EnvioRapidoApi.Models;

namespace EnvioRapidoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Envio> Envios { get; set; } // Tabela de envios
    }
}
