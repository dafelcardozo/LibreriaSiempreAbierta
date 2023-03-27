using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avenue17.Migrations
{
    /// <inheritdoc />
    public partial class mssqlonprem_migration_166 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_editoriales_nombre_sede",
                table: "editoriales");

            migrationBuilder.DropIndex(
                name: "IX_autores_nombre_apellidos",
                table: "autores");

            migrationBuilder.CreateIndex(
                name: "IX_editoriales_nombre_sede",
                table: "editoriales",
                columns: new[] { "nombre", "sede" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_autores_nombre_apellidos",
                table: "autores",
                columns: new[] { "nombre", "apellidos" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_editoriales_nombre_sede",
                table: "editoriales");

            migrationBuilder.DropIndex(
                name: "IX_autores_nombre_apellidos",
                table: "autores");

            migrationBuilder.CreateIndex(
                name: "IX_editoriales_nombre_sede",
                table: "editoriales",
                columns: new[] { "nombre", "sede" });

            migrationBuilder.CreateIndex(
                name: "IX_autores_nombre_apellidos",
                table: "autores",
                columns: new[] { "nombre", "apellidos" });
        }
    }
}
