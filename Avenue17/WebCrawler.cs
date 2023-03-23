using Avenue17.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

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
            public string Description { get; set; } = "";
            public List<string> Authors { get; set; } = new List<string>();
            public List<int> AuthorsIdentifiers = new List<int>();

            public string Publisher { get; set; } = "";

            public string Title { get; set; } = "";
            public int PageCount { get; set; } 
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

        public async Task<List<Book2>> DownloadGoogleBooks()
        {
            string[] categories = "fiction,romance,sex,sf,science-fiction,science,advice,economics,history,politics,geography,war,mathematics,french,colombia,spain,russia,paint,physics,medicine,industry,planes,design,architecture,web,programming".Split(",");
            var books = new List<Book2>();

            using (var wc = new HttpClient())
            {
                foreach (string cat in categories)
                {
                    string url = $"https://www.googleapis.com/books/v1/volumes?q=subject:{cat}&maxResults=40";
                    var response = await wc.GetFromJsonAsync<Volume>(url);
                    books.AddRange(response.items);
                }
            }
            //Console.WriteLine(JsonSerializer.Serialize(books));
            Console.WriteLine($"Books: {books.Count}");
            return books;
        }
        private string Truncate(string s, int maxLength) {
            return s.Length <= maxLength?s: s.Substring(0, maxLength);
        }
        public async Task<string> SaveBooks()
        {
            var books = await DownloadGoogleBooks();
            var authors = new HashSet<string>();
           
            foreach (var b in books) {
                authors.AddRange(b.VolumeInfo.Authors);
            }
            Console.WriteLine($"Authors: {authors.Count}");
            var publishers = new HashSet<string>();
            foreach (var b in books){
                var p = Truncate(b.VolumeInfo.Publisher, 45);
                publishers.Add(p);
            }
            Console.WriteLine($"Publishers: {publishers.Count}");

            foreach (var p in publishers)
            {
                _context.Editorial.Add(new Editorial() { Location="Somewhere on the web", Name=p});
            }
            _context.SaveChanges();
            foreach (var a in authors) {
                var names = a.Split(" ");
                var na = new Author() { Name = Truncate(names[0], 45), LastName = Truncate(string.Join(' ', names[1..]), 45) };
                _context.Author.Add(na);
            }
            _context.SaveChanges();
            var isbnHits = new HashSet<long>();
            foreach (var b in books)
            {
                if (b.VolumeInfo.Authors.Count == 0)
                    continue;
                if (b.VolumeInfo.IndustryIdentifiers.Count == 0)
                    continue;
                string first = b.VolumeInfo.Authors[0];
                var authorsA = from Author in _context.Author
                               where first.Contains(Author.Name) && first.Contains(Author.LastName)
                               select Author;
                var editorialsA = from Editorial in _context.Editorial where b.VolumeInfo.Publisher.StartsWith(Editorial.Name) select Editorial;
                if (editorialsA.Count() == 0)
                    continue;
                if (authorsA.Count() == 0)
                    continue;
                string id = b.VolumeInfo.IndustryIdentifiers[0].Identifier;
                Regex regex = new Regex("\\d*");
                var matches = regex.Matches(id);
                if (matches.IsNullOrEmpty())
                    continue;
                string v = matches.First().Value;
                if (v.IsNullOrEmpty())
                    continue;
                long isbn = long.Parse(v);
                if (isbnHits.Contains(isbn))
                    continue;
                try
                {
                    var book = new Book() { Authors = authorsA.ToList(), Editorial = editorialsA.First(), Isbn = isbn, Synopsis = b.VolumeInfo.Description, Title = b.VolumeInfo.Title, NPages = b.VolumeInfo.PageCount };
                    Console.Write(".");
                    _context.Add(book);
                    isbnHits.Add(book.Isbn);
                }
                catch (Exception e){

                    Console.WriteLine(isbn);
                    Console.WriteLine(e);
                }
               
                
               
            }
            _context.SaveChanges();
            return "Done";
        }
    }
}
