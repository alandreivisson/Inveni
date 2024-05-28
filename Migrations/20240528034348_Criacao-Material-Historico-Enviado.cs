using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveni.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoMaterialHistoricoEnviado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorito_TematicaMestre_TematicaMestreId",
                table: "Favorito");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorito_TematicaMestre_TematicaMestreId1",
                table: "Favorito");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorito_Usuario_AprendizId",
                table: "Favorito");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialMatricula_Matricula_MatriculaId",
                table: "MaterialMatricula");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialMatriculaMestre_MatriculaMestre_MatriculaMestreId",
                table: "MaterialMatriculaMestre");

            migrationBuilder.DropForeignKey(
                name: "FK_Matricula_Usuario_AprendizId",
                table: "Matricula");

            migrationBuilder.DropForeignKey(
                name: "FK_MatriculaMestre_Usuario_AprendizId",
                table: "MatriculaMestre");

            migrationBuilder.DropForeignKey(
                name: "FK_TematicaMestre_Usuario_UsuarioId",
                table: "TematicaMestre");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioPerfil_Usuario_UsuarioId",
                table: "UsuarioPerfil");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorito",
                table: "Favorito");

            migrationBuilder.DropIndex(
                name: "IX_Favorito_TematicaMestreId1",
                table: "Favorito");

            migrationBuilder.DropColumn(
                name: "TematicaMestreId1",
                table: "Favorito");

            migrationBuilder.RenameTable(
                name: "Favorito",
                newName: "Favoritos");

            migrationBuilder.RenameIndex(
                name: "IX_Favorito_TematicaMestreId",
                table: "Favoritos",
                newName: "IX_Favoritos_TematicaMestreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favoritos",
                table: "Favoritos",
                columns: new[] { "AprendizId", "TematicaMestreId" });

            migrationBuilder.CreateTable(
                name: "MaterialEnviadoHistorico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    AprendizId = table.Column<int>(type: "int", nullable: false),
                    MestreId = table.Column<int>(type: "int", nullable: false),
                    DataEnviado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialEnviadoHistorico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialEnviadoHistorico_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialEnviadoHistorico_Usuario_AprendizId",
                        column: x => x.AprendizId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialEnviadoHistorico_AprendizId",
                table: "MaterialEnviadoHistorico",
                column: "AprendizId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialEnviadoHistorico_MaterialId",
                table: "MaterialEnviadoHistorico",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favoritos_TematicaMestre_TematicaMestreId",
                table: "Favoritos",
                column: "TematicaMestreId",
                principalTable: "TematicaMestre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favoritos_Usuario_AprendizId",
                table: "Favoritos",
                column: "AprendizId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialMatricula_Matricula_MatriculaId",
                table: "MaterialMatricula",
                column: "MatriculaId",
                principalTable: "Matricula",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialMatriculaMestre_MatriculaMestre_MatriculaMestreId",
                table: "MaterialMatriculaMestre",
                column: "MatriculaMestreId",
                principalTable: "MatriculaMestre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matricula_Usuario_AprendizId",
                table: "Matricula",
                column: "AprendizId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MatriculaMestre_Usuario_AprendizId",
                table: "MatriculaMestre",
                column: "AprendizId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TematicaMestre_Usuario_UsuarioId",
                table: "TematicaMestre",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioPerfil_Usuario_UsuarioId",
                table: "UsuarioPerfil",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_Favoritos_TematicaMestre_TematicaMestreId",
                table: "Favoritos");

            migrationBuilder.DropForeignKey(
                name: "FK_Favoritos_Usuario_AprendizId",
                table: "Favoritos");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialMatricula_Matricula_MatriculaId",
                table: "MaterialMatricula");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialMatriculaMestre_MatriculaMestre_MatriculaMestreId",
                table: "MaterialMatriculaMestre");

            migrationBuilder.DropForeignKey(
                name: "FK_Matricula_Usuario_AprendizId",
                table: "Matricula");

            migrationBuilder.DropForeignKey(
                name: "FK_MatriculaMestre_Usuario_AprendizId",
                table: "MatriculaMestre");

            migrationBuilder.DropForeignKey(
                name: "FK_TematicaMestre_Usuario_UsuarioId",
                table: "TematicaMestre");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioPerfil_Usuario_UsuarioId",
                table: "UsuarioPerfil");

            migrationBuilder.DropTable(
                name: "MaterialEnviadoHistorico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favoritos",
                table: "Favoritos");

            migrationBuilder.RenameTable(
                name: "Favoritos",
                newName: "Favorito");

            migrationBuilder.RenameIndex(
                name: "IX_Favoritos_TematicaMestreId",
                table: "Favorito",
                newName: "IX_Favorito_TematicaMestreId");

            migrationBuilder.AddColumn<int>(
                name: "TematicaMestreId1",
                table: "Favorito",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorito",
                table: "Favorito",
                columns: new[] { "AprendizId", "TematicaMestreId" });

            migrationBuilder.CreateIndex(
                name: "IX_Favorito_TematicaMestreId1",
                table: "Favorito",
                column: "TematicaMestreId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorito_TematicaMestre_TematicaMestreId",
                table: "Favorito",
                column: "TematicaMestreId",
                principalTable: "TematicaMestre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorito_TematicaMestre_TematicaMestreId1",
                table: "Favorito",
                column: "TematicaMestreId1",
                principalTable: "TematicaMestre",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorito_Usuario_AprendizId",
                table: "Favorito",
                column: "AprendizId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialMatricula_Matricula_MatriculaId",
                table: "MaterialMatricula",
                column: "MatriculaId",
                principalTable: "Matricula",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialMatriculaMestre_MatriculaMestre_MatriculaMestreId",
                table: "MaterialMatriculaMestre",
                column: "MatriculaMestreId",
                principalTable: "MatriculaMestre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matricula_Usuario_AprendizId",
                table: "Matricula",
                column: "AprendizId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatriculaMestre_Usuario_AprendizId",
                table: "MatriculaMestre",
                column: "AprendizId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TematicaMestre_Usuario_UsuarioId",
                table: "TematicaMestre",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioPerfil_Usuario_UsuarioId",
                table: "UsuarioPerfil",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

    }
}
