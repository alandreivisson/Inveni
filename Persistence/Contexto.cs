using Microsoft.EntityFrameworkCore;
using Inveni.Models;

namespace Inveni.Persistence
{
    public class Contexto : DbContext
    {
        //public Contexto() 
        //{
        //}
        public Contexto(DbContextOptions<Contexto> contextOptions) : base(contextOptions)
        {
        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public DbSet<Categoria> Categoria { get; set; } = default!;
        public DbSet<Tematica> Tematica { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tematica>().HasOne(e => e.Categoria).WithMany(e => e.Tematicas).HasForeignKey(e => e.CategoriaId).HasPrincipalKey(e => e.Id);
                  base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().HasMany(u => u.UsuarioPerfil).WithOne(up => up.Usuario).HasForeignKey(up => up.UsuarioId);

            modelBuilder.Entity<Perfil>().HasMany(p => p.UsuarioPerfil).WithOne(up => up.Perfil).HasForeignKey(up => up.PerfilId);

            modelBuilder.Entity<UsuarioPerfil>().HasKey(up => new { up.UsuarioId, up.PerfilId });
        }
    }
}