using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveni.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoTematicaMestreId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TematicaMestreId",
                table: "MatriculaMestre",
                type: "int",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_MatriculaMestre_TematicaMestreId",
                table: "MatriculaMestre",
                column: "TematicaMestreId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatriculaMestre_TematicaMestre_TematicaMestreId",
                table: "MatriculaMestre",
                column: "TematicaMestreId",
                principalTable: "TematicaMestre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
               name: "FK_MatriculaMestre_TematicaMestre_TematicaMestreId",
               table: "MatriculaMestre");

            migrationBuilder.DropIndex(
                name: "IX_MatriculaMestre_TematicaMestreId",
                table: "MatriculaMestre");

            migrationBuilder.DropColumn(
                name: "TematicaMestreId",
                table: "MatriculaMestre");
        }
    }
}
