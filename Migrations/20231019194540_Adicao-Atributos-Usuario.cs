using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveni.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoAtributosUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Biografia",
                table: "Usuario",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Usuario",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biografia",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Usuario");
        }
    }
}
