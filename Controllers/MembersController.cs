using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Models;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly LibraryContext _context;

        public MembersController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            return await _context.Members.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();
            return member;
        }

        [HttpPost]
        public async Task<ActionResult<Member>> CreateMember(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMember), new { id = member.MemberId }, member);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] Member updatedMember)
        {
            try
            {
                var existingMember = await _context.Members.FindAsync(id);
                if (existingMember == null) return NotFound();

                // Validar modelo manualmente
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                // Actualizar solo los campos permitidos
                existingMember.Name = updatedMember.Name;
                existingMember.Email = updatedMember.Email;
                existingMember.CI = updatedMember.CI;
                existingMember.PhoneNumber = updatedMember.PhoneNumber;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Error de concurrencia",
                    Detail = ex.Message,
                    Status = 500
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al actualizar",
                    Detail = ex.Message,
                    Status = 400
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool MemberExists(int id) =>
            _context.Members.Any(e => e.MemberId == id);
    }
}
