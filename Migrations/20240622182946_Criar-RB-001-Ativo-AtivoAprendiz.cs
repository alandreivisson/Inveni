using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveni.Migrations
{
    /// <inheritdoc />
    public partial class CriarRB001AtivoAtivoAprendiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
               name: "Ativo",
               table: "Material",
               type: "bit",
               nullable: false,
               defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "AtivoAprendiz",
                table: "MaterialMatriculaMestre",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "Ativo",
            table: "Material");

            migrationBuilder.DropColumn(
                name: "AtivoAprendiz",
                table: "MaterialMatriculaMestre");

        }
    }
}
