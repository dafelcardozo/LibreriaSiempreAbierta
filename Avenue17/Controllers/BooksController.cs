using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace Avenue17.Controllers
{
    public readonly record struct BookDto(long Isbn, string Title, string Synopsis, int NPages, List<int> Authors, int Editorial);

    public readonly record struct Paginator<T>(int Page, int TotalCount, int TotalPages, int PageSize, IEnumerable<T> Data);

    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksContext _context;

        public BooksController(BooksContext context)
        {
            _context = context;
        }

        Func<Book, object> SortFunction(string sortByField)
        {
            switch (sortByField.ToLower())
            {
                case "title":
                    return (Book b) => b.Title;
                case "isbn":
                    return (Book b) => b.Isbn;
                case "synopsis":
                    return (Book b) => b.Synopsis;
                case "editorial":
                    return (Book b) => b.Editorial.Name;
                case "npages":
                    return (Book b) => b.NPages;
                default:
                    return (Book b) => b.Isbn;
            }
            
        } 

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<Paginator<Book>>> GetBooks(long? isbn, bool asc, int PageSize=20, int Page=0, string search = "", string sortBy="")
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            long l;
            if (isbn == null && long.TryParse(search, out l))
            {
                isbn = l;
                search = "";
            }

            var query = (from b in _context.Books
                         where (isbn == null || (b.Isbn == isbn)) &&
                         (search.IsNullOrEmpty() || b.Title.Contains(search) || b.Synopsis.Contains(search) || b.Authors.Any(a => a.Name.Contains(search) || a.LastName.Contains(search)) || b.Editorial.Name.Contains(search) || b.Editorial.Location.Contains(search))
                         select b).Include("Editorial").Include("Authors");
            var sorted = asc?query.OrderBy(SortFunction(sortBy)):query.OrderByDescending(SortFunction(sortBy));
            var TotalCount = await query.CountAsync();
            return new Paginator<Book>() { Data = sorted.Skip(PageSize * Page).Take(PageSize), PageSize=PageSize, Page=Page, TotalCount=TotalCount, TotalPages=TotalCount/PageSize};
        }
 

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(long id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(long id, Book book)
        {
            if (id != book.Isbn)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(BookDto bookDto)
        {

            if (_context.Books == null)
            {
                return Problem("Entity set 'BloggingContext.Books'  is null.");
            }

            var book = new Book()
            {
                Isbn = bookDto.Isbn,
                NPages = bookDto.NPages,
                Synopsis = bookDto.Synopsis,
                Title = bookDto.Title,
                Editorial = (from e in _context.Editorial where e.Id == bookDto.Editorial select e).First(),
                Authors = (from a in _context.Author where bookDto.Authors.Contains(a.Id) select a).ToList(),
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = bookDto.Isbn }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(long id)
        {
            return (_context.Books?.Any(e => e.Isbn == id)).GetValueOrDefault();
        }
    }
}
