using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjetoConcessionaria.Data;
using ProjetoConcessionaria.Models;
using ProjetoConcessionaria.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoConcessionaria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cards = new DashboardCardsViewModel
            {
                TotalVeiculos = await _context.Veiculos.CountAsync(),
                TotalClientes = await _context.Clientes.CountAsync(),
                TotalVendasMes = await _context.Vendas
                    .Where(v => v.DataVenda.Month == DateTime.Now.Month)
                    .CountAsync(),
                FaturamentoMes = await _context.Vendas
                    .Where(v => v.DataVenda.Month == DateTime.Now.Month)
                    .SumAsync(v => v.ValorFinal)
            };

            var dashboard = new HomeDashboardViewModel
            {
                CardsResumo = cards,
                UltimasVendas = await _context.Vendas
                    .Include(v => v.Cliente)
                    .Include(v => v.Veiculo)
                    .OrderByDescending(v => v.DataVenda)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboard);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
