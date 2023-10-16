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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tematica>().HasOne(e => e.Categoria).WithMany(e => e.Tematicas).HasForeignKey(e => e.CategoriaId).HasPrincipalKey(e => e.Id);

            //modelBuilder.Entity<Usuario>().HasMany(u => u.UsuarioPerfil).WithOne(up => up.Usuario).HasForeignKey(up => up.UsuarioId);

            //modelBuilder.Entity<Perfil>().HasMany(p => p.UsuarioPerfil).WithOne(up => up.Perfil).HasForeignKey(up => up.PerfilId);

            modelBuilder.Entity<UsuarioPerfil>().HasKey(up => new { up.Id });

            modelBuilder.Entity<UsuarioPerfil>().HasOne(up => up.Usuario).WithMany(u => u.UsuarioPerfil).HasForeignKey(up => up.UsuarioId);

            modelBuilder.Entity<UsuarioPerfil>().HasOne(up => up.Perfil).WithMany(p => p.UsuarioPerfil).HasForeignKey(up => up.PerfilId);

            modelBuilder.Entity<TematicaMestre>().HasKey(tm => new { tm.Id });

            modelBuilder.Entity<TematicaMestre>().HasOne(tm => tm.Usuario).WithMany(u => u.TematicaMestre).HasForeignKey(tm => tm.UsuarioId);

            modelBuilder.Entity<TematicaMestre>().HasOne(tm => tm.Tematica).WithMany(t => t.TematicaMestre).HasForeignKey(tm => tm.TematicaId);

            base.OnModelCreating(modelBuilder);

            //var adminUser = modelBuilder.Entity<Usuario>().HasData(new
            //{
            //    Id = 1,
            //    Nome = "Admin",
            //    Email = "admin.inveni@gmail.com",
            //    Senha = "123",
            //    Ativo = 1
            //});

            //// Adicionar o usuário "admin" apenas se ele não existir
            //if (adminUser != null)
            //{
            //    modelBuilder.Entity<Usuario>().HasData(new
            //    {
            //        Id = 1,
            //        Nome = "Admin",
            //        Email = "admin.inveni@gmail.com",
            //        Senha = "123",
            //        Ativo = 1
            //    });
            //}

            //// Verificar se os perfis "Administrador", "Mestre" e "Aprendiz" já existem no banco de dados
            //var adminPerfil = modelBuilder.Entity<Perfil>().HasData(new
            //{
            //    Id = 1,
            //    Descricao = "Administrador"
            //});

            //var mestrePerfil = modelBuilder.Entity<Perfil>().HasData(new
            //{
            //    Id = 2,
            //    Descricao = "Mestre"
            //});

            //var aprendizPerfil = modelBuilder.Entity<Perfil>().HasData(new
            //{
            //    Id = 3,
            //    Descricao = "Aprendiz"
            //});

            //// Adicionar os perfis apenas se eles não existirem
            //if (adminPerfil != null)
            //{
            //    modelBuilder.Entity<Perfil>().HasData(new
            //    {
            //        Id = 1,
            //        Descricao = "Administrador"
            //    });
            //}

            //if (mestrePerfil != null)
            //{
            //    modelBuilder.Entity<Perfil>().HasData(new
            //    {
            //        Id = 2,
            //        Descricao = "Mestre"
            //    });
            //}

            //if (aprendizPerfil != null)
            //{
            //    modelBuilder.Entity<Perfil>().HasData(new
            //    {
            //        Id = 3,
            //        Descricao = "Aprendiz"
            //    });
            //}

            //// Verificar se a relação do usuário "admin" com o perfil "Administrador" já existe no banco de dados
            //var adminUsuarioPerfil = modelBuilder.Entity<UsuarioPerfil>().HasData(new
            //{
            //    UsuarioId = 1,
            //    PerfilId = 1
            //});

            //// Adicionar a relação apenas se ela não existir
            //if (adminUsuarioPerfil != null)
            //{
            //    modelBuilder.Entity<UsuarioPerfil>().HasData(new
            //    {
            //        UsuarioId = 1,
            //        PerfilId = 1
            //    });
            //}
        }
    }
}