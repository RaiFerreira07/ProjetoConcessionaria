using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjetoConcessionaria.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(
                "server=localhost;database=lojacarrosdb;user=root;password=2007",
                new MySqlServerVersion(new Version(12, 0, 2)) // ajuste conforme sua versão do MySQL/MariaDB
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
