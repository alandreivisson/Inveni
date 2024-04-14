﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Inveni.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public required string Nome { get; set; }

        [MaxLength(200)]
        [EmailAddress]
        [Display (Name ="E-mail")]
        public required string Email { get; set; }

        [MaxLength(200)]
        public string? Localizacao { get; set; }

        [MaxLength(200)]
        public string? Biografia { get; set; }

        [MaxLength(200)]
        public string? Telefone { get; set; }

        [PasswordPropertyText]
        public required string Senha { get; set; }

        [MaxLength(500)]
        public string? CaminhoFoto { get; set; }


        [Display(Name = "Ativo")]
        public required bool Ativo { get; set; }

        public virtual ICollection<UsuarioPerfil>? UsuarioPerfil { get; set; }
        public virtual ICollection<TematicaMestre>? TematicaMestre { get; set; }


        public bool ValidarPasswordComplexity()
        {
            // Validar a complexidade da senha
            var regex = new Regex("^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+{}:;|<>,/?\\]]).*$");

            return regex.IsMatch(Senha);
        }
        public virtual ICollection<Favorito>? Favoritos { get; set; }

        public virtual ICollection<Matricula>? Matriculas { get; set; }

        public virtual ICollection<MatriculaMestre>? MatriculaMestreMestre { get; set; }

        public virtual ICollection<MatriculaMestre>? MatriculaMestreAprendiz { get; set; }
    }



}
