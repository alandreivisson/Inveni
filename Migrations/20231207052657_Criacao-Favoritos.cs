using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveni.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoFavoritos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<int>(
                name: "TematicaMestreId",
                table: "Usuario",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Favorito",
                columns: table => new
                {
                    AprendizId = table.Column<int>(type: "int", nullable: false),
                    TematicaMestreId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Favoritado = table.Column<bool>(type: "bit", nullable: false),
                    TematicaMestreId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorito", x => new { x.AprendizId, x.TematicaMestreId });
                    table.ForeignKey(
                        name: "FK_Favorito_TematicaMestre_TematicaMestreId",
                        column: x => x.TematicaMestreId,
                        principalTable: "TematicaMestre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorito_TematicaMestre_TematicaMestreId1",
                        column: x => x.TematicaMestreId1,
                        principalTable: "TematicaMestre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction); // Alterado para NO ACTION
                    table.ForeignKey(
                        name: "FK_Favorito_Usuario_AprendizId",
                        column: x => x.AprendizId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction); // Alterado para NO ACTION
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_TematicaMestreId",
                table: "Usuario",
                column: "TematicaMestreId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorito_TematicaMestreId",
                table: "Favorito",
                column: "TematicaMestreId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorito_TematicaMestreId1",
                table: "Favorito",
                column: "TematicaMestreId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_TematicaMestre_TematicaMestreId",
                table: "Usuario",
                column: "TematicaMestreId",
                principalTable: "TematicaMestre",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction); // Alterado para NO ACTION
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_TematicaMestre_TematicaMestreId",
                table: "Usuario");

            migrationBuilder.DropTable(
                name: "Favorito");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_TematicaMestreId",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "TematicaMestreId",
                table: "Usuario");
        }
    }
}
