using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace Avenue17
{

    public class WebCrawler
    {
        private readonly BooksContext _context;
        public class IndustryIdentifier
        {
            public string Identifier { get; set; }

            public long? ParseIsbn
            {
                get
                {
                    Regex regex = new Regex("\\d*");
                    var matches = regex.Matches(Identifier);
                    if (matches.IsNullOrEmpty())
                        return null;
                    string v = matches.First().Value;
                    if (v.IsNullOrEmpty())
                        return null;
                    return long.Parse(v);
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
            public Editorial EditorialRecord { get; set; }

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
            var books = await DownloadGoogleBooks();
            foreach (var b in books)
            {
                foreach (var p in b.VolumeInfo.Publisher)
                {
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
                }
            }
            
            Console.WriteLine($"Publishers: {_context.Editorial.Count()}");
            foreach (var b in books)
            {
                foreach (var a in b.VolumeInfo.Authors)
                {
                    var names = a.Split(" ");
                    var aaa = new { Name = Truncate(names[0], 45), LastName = Truncate(string.Join(' ', names[1..]), 45) };
                    var re = from aut in _context.Author where aut.Name == aaa.Name && aut.Name == aaa.Name select aut;
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
            var isbnHits = new HashSet<long>();
            foreach (var b in books)
            {
                if (b.VolumeInfo.AuthorRecords.Count == 0)
                    continue;
                if (b.VolumeInfo.EditorialRecord is null)
                    continue;
                var result = from i in b.VolumeInfo.IndustryIdentifiers where i.ParseIsbn is not null select i.ParseIsbn.Value;
                if (!result.Any())
                    continue;
                long isbn = result.First();
                if (isbnHits.Contains(isbn))
                    continue;
                try
                {
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
                    isbnHits.Add(book.Isbn);
                }
                catch (Exception e)
                {
                    Console.WriteLine(isbn);
                    Console.WriteLine(e);
                }
            }
            _context.SaveChanges();
            return "Done";
        }
    }
}
