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
    public class RezervisanaSjedistaController : ControllerBase
    {
        private readonly RezervacijeBioskopskihKarataContext _context;

        public RezervisanaSjedistaController(RezervacijeBioskopskihKarataContext context)
        {
            _context = context;
        }

        // GET: api/RezervisanaSjedista
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RezervisanaSjedista>>> GetRezervisanaSjedista()
        {
            return await _context.RezervisanaSjedista.ToListAsync();
        }

        // GET: api/RezervisanaSjedista/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RezervisanaSjedista>> GetRezervisanaSjedista(int id)
        {
            var rezervisanaSjedista = await _context.RezervisanaSjedista.FindAsync(id);

            if (rezervisanaSjedista == null)
            {
                return NotFound();
            }

            return rezervisanaSjedista;
        }

        // PUT: api/RezervisanaSjedista/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRezervisanaSjedista(int id, RezervisanaSjedista rezervisanaSjedista)
        {
            if (id != rezervisanaSjedista.Id)
            {
                return BadRequest();
            }

            _context.Entry(rezervisanaSjedista).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RezervisanaSjedistaExists(id))
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

        // POST: api/RezervisanaSjedista
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RezervisanaSjedista>> PostRezervisanaSjedista(RezervisanaSjedista rezervisanaSjedista)
        {
            _context.RezervisanaSjedista.Add(rezervisanaSjedista);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRezervisanaSjedista", new { id = rezervisanaSjedista.Id }, rezervisanaSjedista);
        }

        // DELETE: api/RezervisanaSjedista/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRezervisanaSjedista(int id)
        {
            var rezervisanaSjedista = await _context.RezervisanaSjedista.FindAsync(id);
            if (rezervisanaSjedista == null)
            {
                return NotFound();
            }

            _context.RezervisanaSjedista.Remove(rezervisanaSjedista);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RezervisanaSjedistaExists(int id)
        {
            return _context.RezervisanaSjedista.Any(e => e.Id == id);
        }
    }
}
