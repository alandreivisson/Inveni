using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveni.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoColunaMestreIdMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<int>(
                name: "MestreId",
                table: "Material",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Material_MestreId",
                table: "Material",
                column: "MestreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Usuario_MestreId",
                table: "Material",
                column: "MestreId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,  // Modifiquei aqui
                onUpdate: ReferentialAction.NoAction);  // Modifiquei aqui
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_Usuario_MestreId",
                table: "Material");

            migrationBuilder.DropIndex(
                name: "IX_Material_MestreId",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "MestreId",
                table: "Material");
        }
    }
}
