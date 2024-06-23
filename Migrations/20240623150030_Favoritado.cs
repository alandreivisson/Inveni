using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveni.Migrations
{
    /// <inheritdoc />
    public partial class Favoritado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favoritado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AprendizId = table.Column<int>(type: "int", nullable: false),
                    TematicasMestreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favoritado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favoritado_TematicaMestre_TematicasMestreId",
                        column: x => x.TematicasMestreId,
                        principalTable: "TematicaMestre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favoritado_Usuario_AprendizId",
                        column: x => x.AprendizId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favoritado_AprendizId",
                table: "Favoritado",
                column: "AprendizId");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritado_TematicasMestreId",
                table: "Favoritado",
                column: "TematicasMestreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favoritado");
        }
    }
}
