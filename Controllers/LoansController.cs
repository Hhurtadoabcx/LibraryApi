using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Models;

namespace LibraryApi.Controllers
{
    /// Controlador prestamos 
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LoansController(LibraryContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Obtener prestamos.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            return await _context.Loans.Include(l => l.Book).Include(l => l.Member).ToListAsync();
        }
        /// <summary>
        /// Obtener prestamo por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            var loan = await _context.Loans.Include(l => l.Book).Include(l => l.Member)
                .FirstOrDefaultAsync(l => l.LoanId == id);

            if (loan == null) return NotFound();
            return loan;
        }

        [HttpPost]
        public async Task<ActionResult<Loan>> CreateLoan(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLoan), new { id = loan.LoanId }, loan);
        }
        /// <summary>
        /// Actualizar prestamo.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(int id, Loan loan)
        {
            if (id != loan.LoanId) return BadRequest();

            _context.Entry(loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }
        /// <summary>
        /// Eliminar prestamo.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null) return NotFound();

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool LoanExists(int id) =>
            _context.Loans.Any(e => e.LoanId == id);

        /// <summary>
        /// Hacer prestamo.
        /// </summary>
        /// <param name="memberId">ID del miembro</param>
        /// <param name="bookId">ID del libro</param>
        [HttpPost("loan")]
        public async Task<IActionResult> LoanBook(
            [FromQuery] int memberId,
            [FromQuery] int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return NotFound("Libro no encontrado");

            if (!book.IsAvailable)
                return BadRequest("El libro no está disponible");

            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
                return NotFound("Miembro no encontrado");

            var loan = new Loan
            {
                MemberId = memberId,
                BookId = bookId,
                LoanDate = DateTime.Now
            };

            book.IsAvailable = false;

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return Ok("Préstamo registrado correctamente");
        }

        /// <summary>
        /// Devolver librp
        /// </summary>
        /// <param name="loanId">ID del préstamo</param>
        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromQuery] int loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null)
                return NotFound("Préstamo no encontrado");

            if (loan.ReturnDate != null)
                return BadRequest("Este préstamo ya fue devuelto");

            loan.ReturnDate = DateTime.Now;

            var book = await _context.Books.FindAsync(loan.BookId);
            if (book != null)
                book.IsAvailable = true;

            await _context.SaveChangesAsync();

            return Ok("Devolución registrada correctamente");
        }
        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetLoansByMember(int memberId)
        {
            // Validación mejorada
            if (memberId <= 0) return BadRequest("ID de miembro inválido");

            var memberExists = await _context.Members.AnyAsync(m => m.MemberId == memberId);
            if (!memberExists) return NotFound("Miembro no encontrado");

            var loans = await _context.Loans
                .Include(l => l.Book)
                .Where(l => l.MemberId == memberId)
                .Select(l => new
                {
                    l.LoanId,
                    l.BookId,
                    l.Book.Title,
                    l.LoanDate,
                    l.ReturnDate
                })
                .ToListAsync();

            return Ok(loans);
        }

    }
}
