using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avenue17.Migrations
{
    /// <inheritdoc />
    public partial class mssqlonprem_migration_651 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "n_paginas",
                table: "libros",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_autores_nombre_apellidos",
                table: "autores",
                columns: new[] { "nombre", "apellidos" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_autores_nombre_apellidos",
                table: "autores");

            migrationBuilder.AlterColumn<string>(
                name: "n_paginas",
                table: "libros",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
