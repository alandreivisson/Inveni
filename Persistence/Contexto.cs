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
        public DbSet<TematicaMestre> TematicaMestre { get; set; }
        public DbSet<Modelo> Modelo { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tematica>().HasOne(e => e.Categoria).WithMany(e => e.Tematicas).HasForeignKey(e => e.CategoriaId).HasPrincipalKey(e => e.Id);

            modelBuilder.Entity<UsuarioPerfil>().HasKey(up => new { up.Id });

            modelBuilder.Entity<UsuarioPerfil>().HasOne(up => up.Usuario).WithMany(u => u.UsuarioPerfil).HasForeignKey(up => up.UsuarioId);

            modelBuilder.Entity<UsuarioPerfil>().HasOne(up => up.Perfil).WithMany(p => p.UsuarioPerfil).HasForeignKey(up => up.PerfilId);

            modelBuilder.Entity<TematicaMestre>().HasKey(tm => new { tm.Id });

            modelBuilder.Entity<TematicaMestre>().HasOne(tm => tm.Usuario).WithMany(u => u.TematicaMestre).HasForeignKey(tm => tm.UsuarioId);

            modelBuilder.Entity<TematicaMestre>().HasOne(tm => tm.Tematica).WithMany(t => t.TematicaMestre).HasForeignKey(tm => tm.TematicaId);

            modelBuilder.Entity<TematicaMestre>().HasOne(tm => tm.Modelo).WithMany(m => m.TematicaMestre).HasForeignKey(tm => tm.ModeloId);

            base.OnModelCreating(modelBuilder);
        }
    }
}