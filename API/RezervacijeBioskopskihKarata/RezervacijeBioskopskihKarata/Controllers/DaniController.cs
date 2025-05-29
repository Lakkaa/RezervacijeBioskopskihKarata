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
    public class DaniController : ControllerBase
    {
        private readonly RezervacijeBioskopskihKarataContext _context;

        public DaniController(RezervacijeBioskopskihKarataContext context)
        {
            _context = context;
        }

        // GET: api/Dani
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dani>>> GetDani()
        {
            return await _context.Dani.ToListAsync();
        }

        // GET: api/Dani/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dani>> GetDani(int id)
        {
            var dani = await _context.Dani.FindAsync(id);

            if (dani == null)
            {
                return NotFound();
            }

            return dani;
        }

        // PUT: api/Dani/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDani(int id, Dani dani)
        {
            if (id != dani.DanId)
            {
                return BadRequest();
            }

            _context.Entry(dani).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DaniExists(id))
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

        // POST: api/Dani
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dani>> PostDani(Dani dani)
        {
            _context.Dani.Add(dani);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDani", new { id = dani.DanId }, dani);
        }

        // DELETE: api/Dani/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDani(int id)
        {
            var dani = await _context.Dani.FindAsync(id);
            if (dani == null)
            {
                return NotFound();
            }

            _context.Dani.Remove(dani);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DaniExists(int id)
        {
            return _context.Dani.Any(e => e.DanId == id);
        }
    }
}
