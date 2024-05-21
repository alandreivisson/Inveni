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
           // Database.EnsureCreated();
        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public DbSet<Categoria> Categoria { get; set; } = default!;
        public DbSet<Tematica> Tematica { get; set; } = default!;
        public DbSet<TematicaMestre> TematicaMestre { get; set; }
        public DbSet<Modelo> Modelo { get; set; }
        public DbSet<Matricula> Matricula { get; set; }
        public DbSet<Material>  Material { get; set; }
        public DbSet<MaterialMatricula> MaterialMatricula { get; set; }
        public DbSet<MatriculaMestre> MatriculaMestre { get; set; }
        public DbSet<MaterialMatriculaMestre> MaterialMatriculaMestre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tematica>().HasOne(e => e.Categoria).WithMany(e => e.Tematicas).HasForeignKey(e => e.CategoriaId).HasPrincipalKey(e => e.Id);

            modelBuilder.Entity<UsuarioPerfil>().HasKey(up => new { up.Id });

            modelBuilder.Entity<UsuarioPerfil>().HasOne(up => up.Usuario).WithMany(u => u.UsuarioPerfil).HasForeignKey(up => up.UsuarioId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsuarioPerfil>().HasOne(up => up.Perfil).WithMany(p => p.UsuarioPerfil).HasForeignKey(up => up.PerfilId);

            modelBuilder.Entity<TematicaMestre>().HasKey(tm => new { tm.Id });

            modelBuilder.Entity<TematicaMestre>().HasOne(tm => tm.Usuario).WithMany(u => u.TematicaMestre).HasForeignKey(tm => tm.UsuarioId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TematicaMestre>().HasOne(tm => tm.Tematica).WithMany(t => t.TematicaMestre).HasForeignKey(tm => tm.TematicaId);

            modelBuilder.Entity<TematicaMestre>().HasOne(tm => tm.Modelo).WithMany(m => m.TematicaMestre).HasForeignKey(tm => tm.ModeloId);

            modelBuilder.Entity<Favorito>().HasKey(f => new { f.AprendizId, f.TematicaMestreId });

            modelBuilder.Entity<Favorito>().HasOne(f => f.Aprendiz).WithMany(a => a.Favoritos).HasForeignKey(f => f.AprendizId);

            modelBuilder.Entity<Favorito>().HasOne(f => f.TematicaMestre).WithMany().HasForeignKey(f => f.TematicaMestreId);

            modelBuilder.Entity<Matricula>().HasKey(m => new { m.Id});

            modelBuilder.Entity<Matricula>().HasOne(m => m.TematicaMestre).WithMany(a => a.Matriculas).HasForeignKey(m => m.TematicaMestreId);

            modelBuilder.Entity<Matricula>().HasOne(m => m.Aprendiz).WithMany(u => u.Matriculas).HasForeignKey(m => m.AprendizId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Material>().HasKey(m => new { m.Id });

            modelBuilder.Entity<MaterialMatricula>().HasKey(mm => new { mm.Id});

            modelBuilder.Entity<MaterialMatricula>().HasOne(mm => mm.Material).WithMany(m => m.MaterialMatricula).HasForeignKey(mm => mm.MaterialId);

            modelBuilder.Entity<MaterialMatricula>().HasOne(mm => mm.Matricula).WithMany(m => m.MaterialMatricula).HasForeignKey(mm => mm.MatriculaId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatriculaMestre>()
            .HasKey(mm => mm.Id);

            modelBuilder.Entity<MatriculaMestre>().HasOne(mm => mm.Mestre).WithMany(u => u.MatriculaMestreMestre).HasForeignKey(mm => mm.MestreId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatriculaMestre>().HasOne(mm => mm.Aprendiz).WithMany(u => u.MatriculaMestreAprendiz).HasForeignKey(mm => mm.AprendizId);

            modelBuilder.Entity<MaterialMatriculaMestre>().HasKey(mm => mm.Id);

            modelBuilder.Entity<MaterialMatriculaMestre>().HasOne(mm => mm.Material).WithMany(m => m.MaterialMatriculaMestre).HasForeignKey(mm => mm.MaterialId);

            modelBuilder.Entity<MaterialMatriculaMestre>().HasOne(mm => mm.MatriculaMestre).WithMany(m => m.MaterialMatriculaMestre).HasForeignKey(mm => mm.MatriculaMestreId).OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}