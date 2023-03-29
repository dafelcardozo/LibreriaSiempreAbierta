using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Globalization;

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
            public string Publisher { get; set; } = "";
            public string Title { get; set; } = "";
            public int PageCount { get; set; }

            public long? BestIsbn {
                get {
                    return (from ii in IndustryIdentifiers where ii.ParseIsbn is not null select ii.ParseIsbn).Max();
                } 
            }
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
            Console.WriteLine($"Initial books count: {books.Count}");
            return books;
        }
        private string Truncate(string s, int maxLength)
        {
            s = s.Trim();
            return s.Length <= maxLength ? s : s.Substring(0, maxLength);
        }
 
        public async Task<string> SaveBooks()
        {
            var books = (await DownloadGoogleBooks()).FindAll(b => b.VolumeInfo.IndustryIdentifiers.Any());
            var distinctPublishers = (from b in books select Truncate(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(b.VolumeInfo.Publisher.ToLower()), 45)).Distinct().Order();
            _context.Editorial.AddRange(from p in distinctPublishers select new Editorial() { Name = p, Location = "Somewhere on the web" });
            _context.SaveChanges();

            var distinctAuthors = (from b in books from a in b.VolumeInfo.Authors select CultureInfo.InvariantCulture.TextInfo.ToTitleCase(a.ToLower())).Distinct();
            var newAuthors = from n in distinctAuthors select n.Split() into names select new Author() { Name = Truncate(names[0], 45), LastName = Truncate(string.Join(' ', names[1..]), 45) };
            _context.Author.AddRange(newAuthors);
            _context.SaveChanges();

            var booksToInsert = from b in books
                                from a in b.VolumeInfo.Authors
                                join editorial in _context.Editorial on Truncate(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(b.VolumeInfo.Publisher.ToLower()), 45) equals editorial.Name
                                join author in _context.Author on new { Name = Truncate(a.Split(' ')[0], 45), LastName = Truncate(string.Join(' ', a.Split(' ')[1..]), 45) } equals new { author.Name, author.LastName }
                                where b.VolumeInfo.BestIsbn is not null
                                group new 
                                { 
                                    Editorial=editorial,
                                    Author=author, 
                                    Isbn=b.VolumeInfo.BestIsbn.Value,
                                    Synopsis=b.VolumeInfo.Description,
                                    b.VolumeInfo.Title,
                                    NPages=b.VolumeInfo.PageCount
                                } by b.VolumeInfo.BestIsbn;
            var range = from b in booksToInsert select new Book() { 
                Isbn = b.First().Isbn, 
                Editorial = b.First().Editorial, 
                NPages = b.First().NPages, 
                Synopsis = b.First().Synopsis, 
                Title = b.First().Title,
                Authors=( from aa in b select aa.Author).ToList() };
            _context.Books.AddRange(range);
            _context.SaveChanges();

            return "Done";
        }
    }
}
