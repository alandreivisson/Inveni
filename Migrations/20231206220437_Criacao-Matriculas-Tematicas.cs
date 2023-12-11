using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveni.Migrations {
    /// <inheritdoc />
    public partial class CriacaoMatriculasTematicas : Migration {/// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
            name: "Matricula",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                AprendizId = table.Column<int>(type: "int", nullable: false),
                TematicaMestreId = table.Column<int>(type: "int", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Matricula", x => x.Id);
                table.ForeignKey(
                    name: "FK_Matricula_TematicaMestre_TematicaMestreId",
                    column: x => x.TematicaMestreId,
                    principalTable: "TematicaMestre",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Matricula_Usuario_AprendizId",
                    column: x => x.AprendizId,
                    principalTable: "Usuario",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.NoAction);
            });

            migrationBuilder.CreateIndex(
                name: "IX_Matricula_AprendizId",
                table: "Matricula",
                column: "AprendizId");

            migrationBuilder.CreateIndex(
                name: "IX_Matricula_TematicaMestreId",
                table: "Matricula",
                column: "TematicaMestreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "Matricula");
        }

    }
}
