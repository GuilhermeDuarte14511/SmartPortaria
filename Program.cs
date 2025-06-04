using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SmartPortaria.Infrastructure.Data;
using SmartPortaria.Application.Interfaces;
using SmartPortaria.Application.Services;
using SmartPortaria.Domain.Interfaces;
using SmartPortaria.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Conex�o com banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsAssembly("SmartPortaria.Infrastructure")
    ));

// Autentica��o via cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/AcessoNegado";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// Sess�o
builder.Services.AddSession();

// Controllers e Views
builder.Services.AddControllersWithViews();

// Inje��o de depend�ncia dos servi�os e reposit�rios
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

var app = builder.Build();

// Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Rota padr�o inicial: Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
