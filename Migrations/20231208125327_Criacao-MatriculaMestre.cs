using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveni.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoMatriculaMestre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatriculaMestre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MestreId = table.Column<int>(type: "int", nullable: false),
                    AprendizId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatriculaMestre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatriculaMestre_Usuario_AprendizId",
                        column: x => x.AprendizId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatriculaMestre_Usuario_MestreId",
                        column: x => x.MestreId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatriculaMestre_AprendizId",
                table: "MatriculaMestre",
                column: "AprendizId");

            migrationBuilder.CreateIndex(
                name: "IX_MatriculaMestre_MestreId",
                table: "MatriculaMestre",
                column: "MestreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatriculaMestre");
        }
    }
}
