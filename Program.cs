using Inveni.Models;
using Inveni.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Contexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Acesso";
        options.AccessDeniedPath = "/Usuarios/ErrorPermissaoAcesso";
        options.Cookie.Name = "Credencial";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy =>
    {
        policy.RequireClaim("Permissoes", "1");
    });
    options.AddPolicy("Mestre", policy =>
    {
        policy.RequireClaim("Permissoes", "2");
    });
    options.AddPolicy("Anonimo", policy =>
    {
        policy.RequireAssertion(context =>
        {
            return !context.User.Identity.IsAuthenticated;
        });
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var contexto = services.GetRequiredService<Contexto>();

    if (!contexto.Modelo.Any(m => m.Descricao == "Híbrido"))
    {
        contexto.Modelo.Add(new Modelo { Descricao = "Híbrido" });
    }

    if (!contexto.Modelo.Any(m => m.Descricao == "Presencial"))
    {
        contexto.Modelo.Add(new Modelo { Descricao = "Presencial" });
    }

    if (!contexto.Modelo.Any(m => m.Descricao == "Remoto"))
    {
        contexto.Modelo.Add(new Modelo { Descricao = "Remoto" });
    }

    if (!contexto.Modelo.Any(m => m.Descricao == "Híbrido-Presencial"))
    {
        contexto.Modelo.Add(new Modelo { Descricao = "Híbrido-Presencial" });
    }

    if (!contexto.Modelo.Any(m => m.Descricao == "Híbrido-Remoto"))
    {
        contexto.Modelo.Add(new Modelo { Descricao = "Híbrido-Remoto" });
    }
   
    if (!contexto.Modelo.Any(m => m.Descricao == "Presencial-Remoto"))
    {
        contexto.Modelo.Add(new Modelo { Descricao = "Presencial-Remoto" });
    }

    if (!contexto.Perfil.Any(p => p.Descricao == "Administrador")) 
    {
        contexto.Perfil.Add(new Perfil { Descricao = "Administrador" });
    }

    if (!contexto.Perfil.Any(p => p.Descricao == "Mestre"))
    {
        contexto.Perfil.Add(new Perfil { Descricao = "Mestre" });
    }

    if (!contexto.Perfil.Any(p => p.Descricao == "Aprendiz"))
    {
        contexto.Perfil.Add(new Perfil { Descricao = "Aprendiz" });
    }
    contexto.SaveChanges();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseStatusCodePagesWithReExecute("/Usuarios/ErrorPaginaNaoEncontrada"); // Redireciona para a ação PaginaNaoEncontrada do controller ErrorPaginaNaoEncontrada em caso de erro 404
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
