using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Avenue17
{

    public class BooksContext : DbContext
    {

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Author { get; set; }   
        public DbSet<Editorial> Editorial { get; set; }
        public BooksContext(DbContextOptions options):base(options)
        {
        }
    }


    [Table("autores")]
    [Index(nameof(Name), nameof(LastName))]
    public class Author
    {
        public int Id { get; set; }

        [Column("nombre")]
        [MaxLength(45)]
        public string Name{ get; set; }
        [Column("apellidos")]
        [MaxLength(45)]
        public string LastName { get; set; }
        
        public List<Book> Books { get; set; } = new List<Book>();
    }
    [Table("editoriales")]
    public class Editorial
    {
        public int Id { get; set; }
        [Column("nombre")]
        [MaxLength(45)]
        public string Name { get; set; } = "";
        [Column("sede")]
        [MaxLength(45)]
        public string Location { get; set; } = "";

        public List<Book> Books { get; set; }= new List<Book>();
    }
    [Table("libros")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Isbn { get; set; }
        [Column("titulo")]
        public string Title { get; set; } = "";
        [Column("sinopsis")]
        public string Synopsis { get; set; } = "";
        [Column("n_paginas")]
        public int NPages { get; set; } 
        [JsonProperty("Authors")]
        public List<Author> Authors { get; set; }=new List<Author>();
        [JsonProperty("Editorial")]
        public required Editorial Editorial { get; set; }
    }
}
