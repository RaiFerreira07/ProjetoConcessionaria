using Microsoft.EntityFrameworkCore;
using ProjetoConcessionaria.Models;

namespace ProjetoConcessionaria.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Veiculo> Veiculos { get; set; } = null!;
        public DbSet<Vendas> Vendas { get; set; } = null!;
    }
}
