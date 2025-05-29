using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RezervacijeBioskopskihKarata.Models;

namespace RezervacijeBioskopskihKarata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerminiController : ControllerBase
    {
        private readonly RezervacijeBioskopskihKarataContext _context;

        public TerminiController(RezervacijeBioskopskihKarataContext context)
        {
            _context = context;
        }

        // GET: api/Termini
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Termini>>> GetTermini()
        {
            return await _context.Termini.ToListAsync();
        }

        // GET: api/Termini/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Termini>> GetTermini(int id)
        {
            var termini = await _context.Termini.FindAsync(id);

            if (termini == null)
            {
                return NotFound();
            }

            return termini;
        }

        // PUT: api/Termini/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTermini(int id, Termini termini)
        {
            if (id != termini.TerminId)
            {
                return BadRequest();
            }

            _context.Entry(termini).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TerminiExists(id))
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

        // POST: api/Termini
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Termini>> PostTermini(Termini termini)
        {
            _context.Termini.Add(termini);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTermini", new { id = termini.TerminId }, termini);
        }

        // DELETE: api/Termini/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTermini(int id)
        {
            var termini = await _context.Termini.FindAsync(id);
            if (termini == null)
            {
                return NotFound();
            }

            _context.Termini.Remove(termini);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TerminiExists(int id)
        {
            return _context.Termini.Any(e => e.TerminId == id);
        }
    }
}
