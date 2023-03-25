using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace Avenue17
{

    public class WebCrawler
    {
        private readonly BooksContext _context;
        public class IndustryIdentifier
        {
            private static readonly Regex regex = new Regex("\\d*");
            public string Identifier { get; set; }

            public long? ParseIsbn
            {
                get
                {
                    MatchCollection matches = regex.Matches(Identifier);
                    if (matches.IsNullOrEmpty())
                        return null;
                    foreach (var m in matches.AsEnumerable())
                    {
                        string v = m.Value;
                        if (v.IsNullOrEmpty())
                            continue;
                        return long.Parse(v);
                    }
                    return null;
                }
            }
        }
        

        public class VolumeInfo
        {
            public List<IndustryIdentifier> IndustryIdentifiers { get; set; } = new List<IndustryIdentifier>();
            public string Description { get; set; } = "";
            public List<string> Authors { get; set; } = new List<string>();
            public List<Author> AuthorRecords { get; } = new List<Author>();

            public string Publisher { get; set; } = "";
            public Editorial? EditorialRecord { get; set; }

            public string Title { get; set; } = "";
            public int PageCount { get; set; }


        }

        public class Book2
        {
            public VolumeInfo VolumeInfo { get; set; }

            public string Category { get; set; }
        }
        public class Volume
        {
            public List<Book2> items { get; set; }
        }
        public WebCrawler(BooksContext context)
        {
            _context = context;
        }
        private readonly string[] categories = @"fiction,romance,sex,sf,science-fiction,science,advice,economics,history,politics,geography,war,mathematics,french,colombia,spain,russia,paint,physics,medicine,industry,planes,design,architecture,web,programming".Split(",");

        public async Task<List<Book2>> DownloadGoogleBooks()
        {
            var books = new List<Book2>();

            using (var wc = new HttpClient())
            {
                foreach (string cat in categories)
                {
                    string url = $"https://www.googleapis.com/books/v1/volumes?q=subject:{cat}&maxResults=40";
                    //var response = await wc.GetAsync(url);
                    //var s = await response.Content.ReadAsStringAsync();
                    //File.AppendAllText(@".\WebCrawlerDebug.json", s);
                    var response = await wc.GetFromJsonAsync<Volume>(url) ?? throw new Exception();
                    response.items.ForEach(i => i.Category = cat);
                    books.AddRange(response.items);
                }
            }
            //Console.WriteLine(JsonSerializer.Serialize(books));
            Console.WriteLine($"Initial books count: {books.Count}");
            return books;
        }
        private string Truncate(string s, int maxLength)
        {
            return s.Length <= maxLength ? s : s.Substring(0, maxLength);
        }

        public void InsertOrUpdate(Author entity)
        {
            _context.Entry(entity).State = entity.Id == 0 ?
                                           EntityState.Added :
                                           EntityState.Modified;
            _context.SaveChanges();
        }
        public async Task<string> SaveBooks()
        {
            var books = (await DownloadGoogleBooks()).FindAll(b => b.VolumeInfo.IndustryIdentifiers.Count > 0);
            books.ForEach(b => {
                var pe = new { Location = "Somewhere on the web", Name = Truncate(b.VolumeInfo.Publisher, 45) };
                var re = from editorial in _context.Editorial where editorial.Location == pe.Location && editorial.Name == pe.Name select editorial;

                if (re.Any())
                {
                    b.VolumeInfo.EditorialRecord = re.First();
                }
                else
                {
                    var editorial = new Editorial() { Location = pe.Location, Name = pe.Name };
                    _context.Editorial.Add(editorial);
                    b.VolumeInfo.EditorialRecord = editorial;
                    _context.SaveChanges();
                }
                if (b.VolumeInfo.EditorialRecord == null)
                    throw new Exception();

            });

            foreach (var b in books)
            {
                foreach (var a in b.VolumeInfo.Authors)
                {
                    var names = a.Split(" ");
                    var aaa = new { Name = Truncate(names[0], 45), LastName = Truncate(string.Join(' ', names[1..]), 45) };
                    var re = from aut in _context.Author where aut.Name == aaa.Name && aut.LastName == aaa.LastName select aut;
                    if (re.Any())
                    {
                        b.VolumeInfo.AuthorRecords.Add(re.First());
                    }
                    else
                    {
                        var na = new Author() { Name = aaa.Name, LastName = aaa.LastName };
                        _context.Author.Add(na);
                        b.VolumeInfo.AuthorRecords.Add(na);
                        _context.SaveChanges();
                    }
                }
            }
            Console.WriteLine($"Authors: {_context.Author.Count()}");

            foreach (var b in books)
            {
                if (b.VolumeInfo.AuthorRecords.IsNullOrEmpty())
                    continue;
                if (b.VolumeInfo.EditorialRecord is null)
                    continue;
                var result = from i in b.VolumeInfo.IndustryIdentifiers where i.ParseIsbn != null && !_context.Books.Any(bb => bb.Isbn == i.ParseIsbn.Value) select i.ParseIsbn.Value;
                if (!result.Any())
                    continue;
                long isbn = result.First();
                var book = new Book()
                {
                    Authors = b.VolumeInfo.AuthorRecords,
                    Editorial = b.VolumeInfo.EditorialRecord,
                    Isbn = isbn,
                    Synopsis = b.VolumeInfo.Description,
                    Title = b.VolumeInfo.Title,
                    NPages = b.VolumeInfo.PageCount
                };
                Console.Write(".");
                _context.Add(book);
                _context.SaveChanges();
            }
            
            return "Done";
        }
    }
}
