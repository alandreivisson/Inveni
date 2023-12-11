﻿// <auto-generated />
using System;
using Inveni.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Inveni.Migrations
{
    [DbContext(typeof(Contexto))]
    [Migration("20231209013552_Criacao-Coluna-MestreId-Material")]
    partial class CriacaoColunaMestreIdMaterial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Inveni.Models.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("Inveni.Models.Favorito", b =>
                {
                    b.Property<int>("AprendizId")
                        .HasColumnType("int");

                    b.Property<int>("TematicaMestreId")
                        .HasColumnType("int");

                    b.Property<bool>("Favoritado")
                        .HasColumnType("bit");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int?>("TematicaMestreId1")
                        .HasColumnType("int");

                    b.HasKey("AprendizId", "TematicaMestreId");

                    b.HasIndex("TematicaMestreId");

                    b.HasIndex("TematicaMestreId1");

                    b.ToTable("Favorito");
                });

            modelBuilder.Entity("Inveni.Models.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CaminhoArquivo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MestreId")
                        .HasColumnType("int");

                    b.Property<string>("NomeArquivo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MestreId");

                    b.ToTable("Material");
                });

            modelBuilder.Entity("Inveni.Models.MaterialMatricula", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<int>("MatriculaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.HasIndex("MatriculaId");

                    b.ToTable("MaterialMatricula");
                });

            modelBuilder.Entity("Inveni.Models.MaterialMatriculaMestre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<int>("MatriculaMestreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.HasIndex("MatriculaMestreId");

                    b.ToTable("MaterialMatriculaMestre");
                });

            modelBuilder.Entity("Inveni.Models.Matricula", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AprendizId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TematicaMestreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AprendizId");

                    b.HasIndex("TematicaMestreId");

                    b.ToTable("Matricula");
                });

            modelBuilder.Entity("Inveni.Models.MatriculaMestre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AprendizId")
                        .HasColumnType("int");

                    b.Property<int>("MestreId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AprendizId");

                    b.HasIndex("MestreId");

                    b.ToTable("MatriculaMestre");
                });

            modelBuilder.Entity("Inveni.Models.Modelo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Modelo");
                });

            modelBuilder.Entity("Inveni.Models.Perfil", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Perfil");
                });

            modelBuilder.Entity("Inveni.Models.Tematica", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.ToTable("Tematica");
                });

            modelBuilder.Entity("Inveni.Models.TematicaMestre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("Biografia")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("ModeloId")
                        .HasColumnType("int");

                    b.Property<int>("TematicaId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ModeloId");

                    b.HasIndex("TematicaId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("TematicaMestre");
                });

            modelBuilder.Entity("Inveni.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("Biografia")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefone")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("TematicaMestreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TematicaMestreId");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("Inveni.Models.UsuarioPerfil", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PerfilId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PerfilId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("UsuarioPerfil");
                });

            modelBuilder.Entity("Inveni.Models.Favorito", b =>
                {
                    b.HasOne("Inveni.Models.Usuario", "Aprendiz")
                        .WithMany("Favoritos")
                        .HasForeignKey("AprendizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inveni.Models.TematicaMestre", "TematicaMestre")
                        .WithMany()
                        .HasForeignKey("TematicaMestreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inveni.Models.TematicaMestre", null)
                        .WithMany("Favoritos")
                        .HasForeignKey("TematicaMestreId1");

                    b.Navigation("Aprendiz");

                    b.Navigation("TematicaMestre");
                });

            modelBuilder.Entity("Inveni.Models.Material", b =>
                {
                    b.HasOne("Inveni.Models.Usuario", "Mestre")
                        .WithMany()
                        .HasForeignKey("MestreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mestre");
                });

            modelBuilder.Entity("Inveni.Models.MaterialMatricula", b =>
                {
                    b.HasOne("Inveni.Models.Material", "Material")
                        .WithMany("MaterialMatricula")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inveni.Models.Matricula", "Matricula")
                        .WithMany("MaterialMatricula")
                        .HasForeignKey("MatriculaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("Matricula");
                });

            modelBuilder.Entity("Inveni.Models.MaterialMatriculaMestre", b =>
                {
                    b.HasOne("Inveni.Models.Material", "Material")
                        .WithMany("MaterialMatriculaMestre")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inveni.Models.MatriculaMestre", "MatriculaMestre")
                        .WithMany("MaterialMatriculaMestre")
                        .HasForeignKey("MatriculaMestreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("MatriculaMestre");
                });

            modelBuilder.Entity("Inveni.Models.Matricula", b =>
                {
                    b.HasOne("Inveni.Models.Usuario", "Aprendiz")
                        .WithMany("Matriculas")
                        .HasForeignKey("AprendizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inveni.Models.TematicaMestre", "TematicaMestre")
                        .WithMany("Matriculas")
                        .HasForeignKey("TematicaMestreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aprendiz");

                    b.Navigation("TematicaMestre");
                });

            modelBuilder.Entity("Inveni.Models.MatriculaMestre", b =>
                {
                    b.HasOne("Inveni.Models.Usuario", "Aprendiz")
                        .WithMany("MatriculaMestreAprendiz")
                        .HasForeignKey("AprendizId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Inveni.Models.Usuario", "Mestre")
                        .WithMany("MatriculaMestreMestre")
                        .HasForeignKey("MestreId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Aprendiz");

                    b.Navigation("Mestre");
                });

            modelBuilder.Entity("Inveni.Models.Tematica", b =>
                {
                    b.HasOne("Inveni.Models.Categoria", "Categoria")
                        .WithMany("Tematicas")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("Inveni.Models.TematicaMestre", b =>
                {
                    b.HasOne("Inveni.Models.Modelo", "Modelo")
                        .WithMany("TematicaMestre")
                        .HasForeignKey("ModeloId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inveni.Models.Tematica", "Tematica")
                        .WithMany("TematicaMestre")
                        .HasForeignKey("TematicaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inveni.Models.Usuario", "Usuario")
                        .WithMany("TematicaMestre")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Modelo");

                    b.Navigation("Tematica");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Inveni.Models.Usuario", b =>
                {
                    b.HasOne("Inveni.Models.TematicaMestre", null)
                        .WithMany("Usuarios")
                        .HasForeignKey("TematicaMestreId");
                });

            modelBuilder.Entity("Inveni.Models.UsuarioPerfil", b =>
                {
                    b.HasOne("Inveni.Models.Perfil", "Perfil")
                        .WithMany("UsuarioPerfil")
                        .HasForeignKey("PerfilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inveni.Models.Usuario", "Usuario")
                        .WithMany("UsuarioPerfil")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Perfil");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Inveni.Models.Categoria", b =>
                {
                    b.Navigation("Tematicas");
                });

            modelBuilder.Entity("Inveni.Models.Material", b =>
                {
                    b.Navigation("MaterialMatricula");

                    b.Navigation("MaterialMatriculaMestre");
                });

            modelBuilder.Entity("Inveni.Models.Matricula", b =>
                {
                    b.Navigation("MaterialMatricula");
                });

            modelBuilder.Entity("Inveni.Models.MatriculaMestre", b =>
                {
                    b.Navigation("MaterialMatriculaMestre");
                });

            modelBuilder.Entity("Inveni.Models.Modelo", b =>
                {
                    b.Navigation("TematicaMestre");
                });

            modelBuilder.Entity("Inveni.Models.Perfil", b =>
                {
                    b.Navigation("UsuarioPerfil");
                });

            modelBuilder.Entity("Inveni.Models.Tematica", b =>
                {
                    b.Navigation("TematicaMestre");
                });

            modelBuilder.Entity("Inveni.Models.TematicaMestre", b =>
                {
                    b.Navigation("Favoritos");

                    b.Navigation("Matriculas");

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Inveni.Models.Usuario", b =>
                {
                    b.Navigation("Favoritos");

                    b.Navigation("MatriculaMestreAprendiz");

                    b.Navigation("MatriculaMestreMestre");

                    b.Navigation("Matriculas");

                    b.Navigation("TematicaMestre");

                    b.Navigation("UsuarioPerfil");
                });
#pragma warning restore 612, 618
        }
    }
}
