using Microsoft.EntityFrameworkCore;
using ProjetoConcessionaria.Data; // garante que o AppDbContext seja reconhecido
using ProjetoConcessionaria.Services;
 // para o TesteBanco

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<OllamaService>();
builder.Services.AddControllersWithViews();

//registra o DbContext com a connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("ConcessionariaDB"),
        new MySqlServerVersion(new Version(12, 0, 2)) // troque pela versão do seu MariaDB
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
