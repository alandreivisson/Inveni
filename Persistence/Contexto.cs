using Microsoft.EntityFrameworkCore;
using Inveni.Models;

namespace Inveni.Persistence {
    public class Contexto : DbContext {
        //public Contexto() 
        //{
        //}
        public Contexto(DbContextOptions<Contexto> contextOptions) : base(contextOptions) {
           //  Database.EnsureCreated();
        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public DbSet<Categoria> Categoria { get; set; } = default!;
        public DbSet<Tematica> Tematica { get; set; } = default!;
        public DbSet<TematicaMestre> TematicaMestre { get; set; }
        public DbSet<TematicaMestre> TematicaMestres { get; set; }
        public DbSet<Modelo> Modelo { get; set; }
        public DbSet<Matricula> Matricula { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<MaterialMatricula> MaterialMatricula { get; set; }
        public DbSet<MatriculaMestre> MatriculaMestre { get; set; }
        public DbSet<MaterialMatriculaMestre> MaterialMatriculaMestre { get; set; }
        public DbSet<MaterialEnviadoHistorico> MaterialEnviadoHistorico { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }
        public DbSet<MatriculaMestre> MatriculaMestreMestre { get; set; }
        public DbSet<MatriculaMestre> MatriculaMestreAprendiz { get; set; }
        public DbSet<Notificacao> Notificacao { get; set; }
        public DbSet<Favoritado> Favoritado { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Tematica
            modelBuilder.Entity<Tematica>()
                .HasOne(e => e.Categoria)
                .WithMany(e => e.Tematicas)
                .HasForeignKey(e => e.CategoriaId)
                .HasPrincipalKey(e => e.Id);

            // UsuarioPerfil
            modelBuilder.Entity<UsuarioPerfil>()
                .HasKey(up => new { up.Id });

            modelBuilder.Entity<UsuarioPerfil>()
                .HasOne(up => up.Usuario)
                .WithMany(u => u.UsuarioPerfil)
                .HasForeignKey(up => up.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsuarioPerfil>()
                .HasOne(up => up.Perfil)
                .WithMany(p => p.UsuarioPerfil)
                .HasForeignKey(up => up.PerfilId);

            // TematicaMestre
            modelBuilder.Entity<TematicaMestre>()
                .HasKey(tm => new { tm.Id });

            modelBuilder.Entity<TematicaMestre>()
                .HasOne(tm => tm.Usuario)
                .WithMany(u => u.TematicaMestres)
                .HasForeignKey(tm => tm.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TematicaMestre>()
                .HasOne(tm => tm.Tematica)
                .WithMany(t => t.TematicaMestre)
                .HasForeignKey(tm => tm.TematicaId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TematicaMestre>()
                .HasOne(tm => tm.Modelo)
                .WithMany(m => m.TematicaMestre)
                .HasForeignKey(tm => tm.ModeloId)
                .OnDelete(DeleteBehavior.Restrict);


            // Favorito
            modelBuilder.Entity<Favorito>()
                .HasKey(f => new { f.AprendizId, f.TematicasMestreId });

            modelBuilder.Entity<Favorito>()
                .HasOne(f => f.Aprendiz)
                .WithMany(a => a.Favoritos)
                .HasForeignKey(f => f.AprendizId);

            modelBuilder.Entity<Favorito>()
                .HasOne(f => f.TematicaMestre)
                .WithMany(tm => tm.Favoritos)
                .HasForeignKey(f => f.TematicasMestreId);

            // Matricula
            modelBuilder.Entity<Matricula>()
                .HasKey(m => new { m.Id });

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.TematicaMestre)
                .WithMany(tm => tm.Matriculas)
                .HasForeignKey(m => m.TematicasMestreId);

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Aprendiz)
                .WithMany(u => u.Matriculas)
                .HasForeignKey(m => m.AprendizId)
                .OnDelete(DeleteBehavior.Restrict);

            // Material
            modelBuilder.Entity<Material>()
                .HasKey(m => new { m.Id });

            // MaterialMatricula
            modelBuilder.Entity<MaterialMatricula>()
                .HasKey(mm => new { mm.Id });

            modelBuilder.Entity<MaterialMatricula>()
                .HasOne(mm => mm.Material)
                .WithMany(m => m.MaterialMatricula)
                .HasForeignKey(mm => mm.MaterialId);

            modelBuilder.Entity<MaterialMatricula>()
                .HasOne(mm => mm.Matricula)
                .WithMany(m => m.MaterialMatricula)
                .HasForeignKey(mm => mm.MatriculaId)
                .OnDelete(DeleteBehavior.Restrict);

            // MatriculaMestre
            modelBuilder.Entity<TematicaMestre>().HasOne(tm => tm.Usuario).WithMany(u => u.TematicaMestres).HasForeignKey(tm => tm.UsuarioId);

            modelBuilder.Entity<TematicaMestre>().HasOne(tm => tm.Tematica).WithMany(t => t.TematicaMestre).HasForeignKey(tm => tm.TematicaId);

            modelBuilder.Entity<TematicaMestre>().HasOne(tm => tm.Modelo).WithMany(m => m.TematicaMestre).HasForeignKey(tm => tm.ModeloId);

            modelBuilder.Entity<MatriculaMestre>()
                .HasKey(mm => mm.Id);

            modelBuilder.Entity<MatriculaMestre>()
                .HasOne(mm => mm.Mestre)
                .WithMany(u => u.MatriculaMestreMestre)
                .HasForeignKey(mm => mm.MestreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatriculaMestre>()
                .HasOne(mm => mm.Aprendiz)
                .WithMany(u => u.MatriculaMestreAprendiz)
                .HasForeignKey(mm => mm.AprendizId)
                .OnDelete(DeleteBehavior.Restrict);

            // MaterialMatriculaMestre
            modelBuilder.Entity<MaterialMatriculaMestre>()
                .HasKey(mm => mm.Id);

            modelBuilder.Entity<MaterialMatriculaMestre>()
                .HasOne(mm => mm.Material)
                .WithMany(m => m.MaterialMatriculaMestre)
                .HasForeignKey(mm => mm.MaterialId);

            modelBuilder.Entity<MaterialMatriculaMestre>()
                .HasOne(mm => mm.MatriculaMestre)
                .WithMany(m => m.MaterialMatriculaMestre)
                .HasForeignKey(mm => mm.MatriculaMestreId)
                .OnDelete(DeleteBehavior.Restrict);

            // MaterialEnviadoHistorico
            modelBuilder.Entity<MaterialEnviadoHistorico>()
                .HasKey(meh => meh.Id);

            modelBuilder.Entity<MaterialEnviadoHistorico>()
                .HasOne(meh => meh.Material)
                .WithMany(m => m.MaterialEnviadoHistorico)
                .HasForeignKey(meh => meh.MaterialId);

            modelBuilder.Entity<MaterialEnviadoHistorico>()
                .HasOne(meh => meh.Aprendiz)
                .WithMany(a => a.MaterialEnviadoHistorico)
                .HasForeignKey(meh => meh.AprendizId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notificacao>().HasKey(meh => meh.Id);

            modelBuilder.Entity<Notificacao>()
                .HasOne(meh => meh.Usuario)
                .WithMany(m => m.Notificacao)
                .HasForeignKey(meh => meh.UsuarioId);

            modelBuilder.Entity<Favoritado>().HasKey(meh => meh.Id);

            modelBuilder.Entity<Favoritado>()
                .HasOne(meh => meh.Aprendiz)
                .WithMany(m => m.Favoritado)
                .HasForeignKey(meh => meh.AprendizId);

            modelBuilder.Entity<Favoritado>()
                .HasOne(meh => meh.TematicaMestre)
                .WithMany(m => m.Favoritado)
                .HasForeignKey(meh => meh.TematicasMestreId);
            base.OnModelCreating(modelBuilder);
        }
    }
}