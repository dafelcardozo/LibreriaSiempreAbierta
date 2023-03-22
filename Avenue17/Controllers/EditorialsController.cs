using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Avenue17.Controllers;

namespace Avenue17.Controllers
{

    public class EditorialDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";

        public int Nbooks { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EditorialsController : ControllerBase
    {
        private readonly BooksContext _context;

        public EditorialsController(BooksContext context)
        {
            _context = context;
        }

        

        // GET: api/Editorials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EditorialDto>>> GetEditorial(string? search="")
        {
          if (_context.Editorial == null)
          {
              return NotFound();
          }
            return await (from e in _context.Editorial where e.Name.Contains(search) || e.Location.Contains(search) select new EditorialDto() { Id = e.Id, Name = e.Name, Location = e.Location, Nbooks = e.Books.Count }).ToListAsync();
        }

        // GET: api/Editorials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Editorial>> GetEditorial(int id)
        {
          if (_context.Editorial == null)
          {
              return NotFound();
          }
            var editorial = await _context.Editorial.FindAsync(id);

            if (editorial == null)
            {
                return NotFound();
            }

            return editorial;
        }

        // PUT: api/Editorials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEditorial(int id, Editorial editorial)
        {
            if (id != editorial.Id)
            {
                return BadRequest();
            }

            _context.Entry(editorial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EditorialExists(id))
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

        // POST: api/Editorials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Editorial>> PostEditorial(Editorial editorial)
        {
          if (_context.Editorial == null)
          {
              return Problem("Entity set 'BooksContext.Editorial'  is null.");
          }
            _context.Editorial.Add(editorial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEditorial", new { id = editorial.Id }, editorial);
        }

        // DELETE: api/Editorials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEditorial(int id)
        {
            if (_context.Editorial == null)
            {
                return NotFound();
            }
            var editorial = await _context.Editorial.FindAsync(id);
            if (editorial == null)
            {
                return NotFound();
            }

            _context.Editorial.Remove(editorial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EditorialExists(int id)
        {
            return (_context.Editorial?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
