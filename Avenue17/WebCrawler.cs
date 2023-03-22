using Avenue17.Controllers;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Net;
using System.Text.Json;

namespace Avenue17
{
    
    public class WebCrawler
    {
        private readonly BooksContext _context;
        public class IndustryIdentifier {
            public string Identifier { get; set; }
            
        }

        public class VolumeInfo
        {
            public List<IndustryIdentifier> IndustryIdentifiers { get; set; } = new List<IndustryIdentifier>();
            public string Description { get; set; }
            public List<string> Authors { get; set; } = new List<string>();

            public string Publisher { get; set; }
        }

        public class Book2
        {
            public VolumeInfo VolumeInfo { get; set; }
        }
        public class Volume
        {
            public List<Book2> items { get; set; }
        }
        public WebCrawler(BooksContext context)
        {
            _context = context;
        }

        public async void DownloadBooks()
        {
            string[] categories = "fiction,romance,sex,sf,science-fiction,science,advice,economics,history,politics,geography,war,mathematics,french,colombia,spain,russia,paint,physics,medicine,industry,planes,design,architecture,web,programming".Split(",");
            var books = new List<Book2>();

            using (var wc = new HttpClient())
            {
                foreach (string cat in categories)
                {
                    string url = $"https://www.googleapis.com/books/v1/volumes?q=subject:{cat}&maxResults=40&projection=lite";
                    var response = await wc.GetFromJsonAsync<Volume>(url);
                    books.AddRange(response.items);
                }
            }
            //            Console.WriteLine(JsonSerializer.Serialize(books));
            Console.WriteLine($"Books: {books.Count}");
            var authors = new HashSet<string>();
            
            foreach (var b in books) {
                authors.AddRange(b.VolumeInfo.Authors);
            }
            Console.WriteLine($"Authors: {authors.Count}");
            var publishers = new HashSet<string>();
            foreach (var b in books) {
                publishers.Add(b.VolumeInfo.Publisher);
            }
            Console.WriteLine($"Publishers: {publishers.Count}");

            foreach (var p in publishers)
            {
                _context.Editorial.Add(new Editorial() { Location="Somewhere on the web", Name=p});
            }
            _context.SaveChanges();
            foreach (var a in authors) {
                var names = a.Split(" ");
                _context.Author.Add(new Author() { Name = names[0], LastName = string.Join(' ', names[1..])});
            }
            _context.SaveChanges();
            foreach (var b in books)
            {
                foreach (var a in b.VolumeInfo.Authors) {
                    var authorsA = from Author in _context.Author
                            where a == (Author.Name + ' ' + Author.LastName)
                            select Author;
                    var editorialsA = from Editorial in _context.Editorial where Editorial.Name == b.VolumeInfo.Publisher select Editorial;
                    var book = new Book() { Authors = authorsA.ToList(), Editorial = editorialsA.First(), Isbn = long.Parse(b.VolumeInfo.IndustryIdentifiers[0].Identifier) };
                    _context.Add(book);
                }
            }
            _context.SaveChanges();
        }
    }
}
