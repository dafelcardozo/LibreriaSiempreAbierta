using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avenue17.Migrations
{
    /// <inheritdoc />
    public partial class mssqlonprem_migration_435 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_editoriales_nombre_sede",
                table: "editoriales",
                columns: new[] { "nombre", "sede" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_editoriales_nombre_sede",
                table: "editoriales");
        }
    }
}
