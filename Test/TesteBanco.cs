using ProjetoConcessionaria.Data;
using ProjetoConcessionaria.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjetoConcessionaria.Tests
{
    public static class TesteBanco
    {
        public static void RodarTeste(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(
                configuration.GetConnectionString("ConcessionariaDB"),
                new MySqlServerVersion(new Version(12, 02, 2))
            );

            using var context = new AppDbContext(optionsBuilder.Options);

            if (!context.Database.CanConnect())
            {
                Console.WriteLine("Erro ao conectar com o banco!");
                return;
            }
            Console.WriteLine("Conexão com o banco OK!");

            var cliente = new Cliente { Nome = "Rai Teste", Email = "rai@teste.com" };
            var veiculo = new Veiculo { Modelo = "Civic", Marca = "Honda", Ano = 2020, Preco = 90000 };

            context.Clientes.Add(cliente);
            context.Veiculos.Add(veiculo);
            context.SaveChanges();

            var venda = new Vendas
            {
                ClienteId = cliente.Id,
                VeiculoId = veiculo.Id,
                DataVenda = DateTime.Now,
                ValorFinal = veiculo.Preco
            };

            context.Vendas.Add(venda);
            context.SaveChanges();

            Console.WriteLine("\nClientes:");
            foreach (var c in context.Clientes.ToList())
                Console.WriteLine($"ID: {c.Id}, Nome: {c.Nome}, Email: {c.Email}");

            Console.WriteLine("\nVeículos:");
            foreach (var v in context.Veiculos.ToList())
                Console.WriteLine($"ID: {v.Id}, Modelo: {v.Modelo}, Marca: {v.Marca}, Ano: {v.Ano}, Preço: {v.Preco}");

            Console.WriteLine("\nVendas:");
            foreach (var v in context.Vendas
                                     .Include(x => x.Cliente)
                                     .Include(x => x.Veiculo)
                                     .ToList())
            {
                Console.WriteLine($"VendaID: {v.Id}, Cliente: {v.Cliente.Nome}, Veículo: {v.Veiculo.Modelo}, Data: {v.DataVenda}");
            }
        }
    }
}
