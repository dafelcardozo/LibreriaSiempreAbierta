using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avenue17.Migrations
{
    /// <inheritdoc />
    public partial class mssqlonprem_migration_588 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "autores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    apellidos = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_autores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "editoriales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    sede = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_editoriales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "libros",
                columns: table => new
                {
                    Isbn = table.Column<long>(type: "bigint", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sinopsis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    n_paginas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EditorialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libros", x => x.Isbn);
                    table.ForeignKey(
                        name: "FK_libros_editoriales_EditorialId",
                        column: x => x.EditorialId,
                        principalTable: "editoriales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    BooksIsbn = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsId, x.BooksIsbn });
                    table.ForeignKey(
                        name: "FK_AuthorBook_autores_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_libros_BooksIsbn",
                        column: x => x.BooksIsbn,
                        principalTable: "libros",
                        principalColumn: "Isbn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksIsbn",
                table: "AuthorBook",
                column: "BooksIsbn");

            migrationBuilder.CreateIndex(
                name: "IX_libros_EditorialId",
                table: "libros",
                column: "EditorialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.DropTable(
                name: "autores");

            migrationBuilder.DropTable(
                name: "libros");

            migrationBuilder.DropTable(
                name: "editoriales");
        }
    }
}
