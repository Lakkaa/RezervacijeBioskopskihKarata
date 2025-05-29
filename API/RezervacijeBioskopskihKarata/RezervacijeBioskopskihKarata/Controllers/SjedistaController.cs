using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RezervacijeBioskopskihKarata.Models;
using RezervacijeBioskopskihKarata.ViewModels;

namespace RezervacijeBioskopskihKarata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SjedistaController : ControllerBase
    {
        private readonly RezervacijeBioskopskihKarataContext _context;

        public SjedistaController(RezervacijeBioskopskihKarataContext context)
        {
            _context = context;
        }

        // GET: api/Sjedista
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sjedista>>> GetSjedista()
        {
            return await _context.Sjedista.ToListAsync();
        }

        // GET: api/Sjedista/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sjedista>> GetSjedista(int id)
        {
            var sjedista = await _context.Sjedista.FindAsync(id);

            if (sjedista == null)
            {
                return NotFound();
            }

            return sjedista;
        }

        // PUT: api/Sjedista/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSjedista(int id, Sjedista sjedista)
        {
            if (id != sjedista.SjedisteId)
            {
                return BadRequest();
            }

            _context.Entry(sjedista).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SjedistaExists(id))
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

        // POST: api/Sjedista
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sjedista>> PostSjedista(Sjedista sjedista)
        {
            _context.Sjedista.Add(sjedista);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSjedista", new { id = sjedista.SjedisteId }, sjedista);
        }

        // DELETE: api/Sjedista/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSjedista(int id)
        {
            var sjedista = await _context.Sjedista.FindAsync(id);
            if (sjedista == null)
            {
                return NotFound();
            }

            _context.Sjedista.Remove(sjedista);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Sjedista/Projekcija/5
        [HttpGet("Projekcija/{projekcijaId}")]
        public async Task<ActionResult<IEnumerable<SjedistaViewModel>>> GetSjedistaByProjekcija(int projekcijaId)
        {
            
            var projekcija = await _context.Projekcije
                .Include(p => p.Sala)
                .FirstOrDefaultAsync(p => p.ProjekcijaId == projekcijaId);

            if (projekcija == null)
            {
                return NotFound();
            }

           
            var sjedista = await _context.Sjedista
                .Where(s => s.SalaId == projekcija.SalaId)
                .ToListAsync();

            
            var rezervisana = await _context.RezervisanaSjedista
                .Where(rs => rs.ProjekcijaId == projekcijaId)
                .Select(rs => rs.SjedisteId)
                .ToListAsync();

            
            var result = sjedista.Select(s => new SjedistaViewModel
            {
                SjedisteId = s.SjedisteId,
                SalaId = s.SalaId,
                SalaNaziv = projekcija.Sala.Naziv,
                BrojSjedista = s.BrojSjedista,
                Red = s.Red,
                ProjekcijaId = projekcijaId,
                IsReserved = rezervisana.Contains(s.SjedisteId),
                RezervisanoSjedisteId = rezervisana.Contains(s.SjedisteId) ? 
                    _context.RezervisanaSjedista
                        .FirstOrDefault(rs => rs.ProjekcijaId == projekcijaId && rs.SjedisteId == s.SjedisteId)?.Id : null
            }).ToList();

            return result;
        }

        // GET: api/Sjedista/ZaProjekciju/{projekcijaId}/Status
        [HttpGet("ZaProjekciju/{projekcijaId}/Status")]
        public async Task<ActionResult<IEnumerable<SjedisteStatusViewModel>>> GetSjedistaStatusByProjekcija(int projekcijaId)
        {
            
            var projekcija = await _context.Projekcije
                .Include(p => p.Sala)
                .FirstOrDefaultAsync(p => p.ProjekcijaId == projekcijaId);

            if (projekcija == null)
            {
                return NotFound("Projekcija not found");
            }

            int salaId = projekcija.SalaId;

            
            var sjedista = await _context.Sjedista
                .Where(s => s.SalaId == salaId)
                .ToListAsync();

            
            var rezervisanaSjedista = await _context.RezervisanaSjedista
                .Where(rs => rs.ProjekcijaId == projekcijaId)
                .Select(rs => rs.SjedisteId)
                .ToListAsync();

           
            var sjedistaStatus = sjedista.Select(s => new SjedisteStatusViewModel
            {
                SjedisteId = s.SjedisteId,
                SalaId = s.SalaId,
                BrojSjedista = s.BrojSjedista,
                Red = s.Red,
                Zauzeto = rezervisanaSjedista.Contains(s.SjedisteId)
            }).ToList();

            return sjedistaStatus;
        }

        private bool SjedistaExists(int id)
        {
            return _context.Sjedista.Any(e => e.SjedisteId == id);
        }
    }
}
