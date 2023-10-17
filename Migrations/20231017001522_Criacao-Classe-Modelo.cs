using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveni.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoClasseModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModeloId",
                table: "TematicaMestre",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Modelo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TematicaMestre_ModeloId",
                table: "TematicaMestre",
                column: "ModeloId");

            migrationBuilder.AddForeignKey(
                name: "FK_TematicaMestre_Modelo_ModeloId",
                table: "TematicaMestre",
                column: "ModeloId",
                principalTable: "Modelo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TematicaMestre_Modelo_ModeloId",
                table: "TematicaMestre");

            migrationBuilder.DropTable(
                name: "Modelo");

            migrationBuilder.DropIndex(
                name: "IX_TematicaMestre_ModeloId",
                table: "TematicaMestre");

            migrationBuilder.DropColumn(
                name: "ModeloId",
                table: "TematicaMestre");
        }
    }
}
