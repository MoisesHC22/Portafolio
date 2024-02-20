using Microsoft.EntityFrameworkCore;
using Portafolio.Infraestructura;
using Portafolio.Models;
using Portafolio.Servicios;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Inyeccion de dependecia para enviar correos
builder.Services.AddTransient<IServiceEmailSendGrid, ServiceEmailSendGrid>();


//Inyeccion de Conexion de BD
builder.Services.AddDbContext<PortafolioDBContext>(opciones =>
opciones.UseSqlServer("name=DefaultConnection"));


builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Agregar middleware de sesión
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");

app.Run();
