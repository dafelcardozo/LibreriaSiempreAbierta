using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Avenue17
{

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Author { get; set; }   
        public DbSet<Editorial> Editorial { get; set; }

        public string DbPath { get; }

        public BloggingContext()
        {
        }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
        }
        */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           /*
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            string? v = configuration.GetConnectionString("cadenaLibreria");
            Console.WriteLine("Cadena de conexión desde el 'onConfiguring': "+ v);
            */
            optionsBuilder.UseSqlServer("Server=LAPTOP-NI5DQ9A4\\SQLEXPRESS;Database=LibreriaCasual;Trusted_Connection=SSPI;Trust Server Certificate=true;Encrypt=False");
            //optionsBuilder.UseSqlServer(v);
            
            //optionsBuilder.UseSqlServer("name=ConnectionStrings:cadenaLibreria");
        }
       

    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; } = new();
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }

    [Table("autores")]
    public class Author
    {
        public int Id { get; set; }

        [Column("nombre")]
        public string Name{ get; set; }
        [Column("apellidos")]
        public string LastName { get; set; }

        public List<Book> Books { get; set; }
    }
    [Table("editoriales")]
    public class Editorial
    {
        public int Id { get; set; }
        [Column("name")]
        public string Nombre { get; set;}
        [Column("location")]
        public string Sede { get; set; }

        public List<Book> Books { get; set; }
    }
    [Table("libros")]
    public class Book
    {
        [Key]
        public int Isbn { get; set; }

        [Column("titulo")]
        public string Title { get; set; }
        [Column("sinopsis")]
        public string Synopsis { get; set; }
        [Column("n_paginas")]
        public string NPages { get; set; }

        public List<Author> Authors { get; set; }

        public Editorial Editorial { get; set; }
    }
}
