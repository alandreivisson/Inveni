using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveni.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoMaterialMatriculaMestre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialMatriculaMestre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    MatriculaMestreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialMatriculaMestre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialMatriculaMestre_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialMatriculaMestre_MatriculaMestre_MatriculaMestreId",
                        column: x => x.MatriculaMestreId,
                        principalTable: "MatriculaMestre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMatriculaMestre_MaterialId",
                table: "MaterialMatriculaMestre",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMatriculaMestre_MatriculaMestreId",
                table: "MaterialMatriculaMestre",
                column: "MatriculaMestreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialMatriculaMestre");
        }
    }
}
