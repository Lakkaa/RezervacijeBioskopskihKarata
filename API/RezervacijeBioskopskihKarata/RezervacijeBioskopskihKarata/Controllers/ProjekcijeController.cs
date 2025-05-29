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
    public class ProjekcijeController : ControllerBase
    {
        private readonly RezervacijeBioskopskihKarataContext _context;

        public ProjekcijeController(RezervacijeBioskopskihKarataContext context)
        {
            _context = context;
        }

        // GET: api/Projekcije
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Projekcije>>> GetProjekcije()
        {
            return await _context.Projekcije.ToListAsync();
        }

        // GET: api/Projekcije/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Projekcije>> GetProjekcije(int id)
        {
            var projekcije = await _context.Projekcije.FindAsync(id);

            if (projekcije == null)
            {
                return NotFound();
            }

            return projekcije;
        }

        // PUT: api/Projekcije/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjekcije(int id, Projekcije projekcije)
        {
            if (id != projekcije.ProjekcijaId)
            {
                return BadRequest();
            }

            _context.Entry(projekcije).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjekcijeExists(id))
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

        // POST: api/Projekcije
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Projekcije>> PostProjekcije(Projekcije projekcije)
        {
            _context.Projekcije.Add(projekcije);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjekcije", new { id = projekcije.ProjekcijaId }, projekcije);
        }

        // DELETE: api/Projekcije/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjekcije(int id)
        {
            var projekcije = await _context.Projekcije.FindAsync(id);
            if (projekcije == null)
            {
                return NotFound();
            }

            _context.Projekcije.Remove(projekcije);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjekcijeExists(int id)
        {
            return _context.Projekcije.Any(e => e.ProjekcijaId == id);
        }

        // GET: api/Projekcije/Details/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult<ProjekcijaViewModel>> GetProjekcijaDetails(int id)
        {
            var projekcija = await _context.Projekcije
                .Include(p => p.Film)
                .Include(p => p.Dan)
                .Include(p => p.Termin)
                .Include(p => p.Sala)
                .FirstOrDefaultAsync(p => p.ProjekcijaId == id);

            if (projekcija == null)
            {
                return NotFound();
            }

            
            var totalSeats = await _context.Sjedista
                .Where(s => s.SalaId == projekcija.SalaId)
                .CountAsync();

            
            var reservedSeats = await _context.RezervisanaSjedista
                .Include(rs => rs.Rezervacija)
                .Where(rs => rs.Rezervacija.ProjekcijaId == id)
                .CountAsync();

            var projekcijaViewModel = new ProjekcijaViewModel
            {
                ProjekcijaId = projekcija.ProjekcijaId,
                FilmId = projekcija.FilmId,
                FilmNaziv = projekcija.Film.Naziv,
                FilmOpis = projekcija.Film.Opis ?? string.Empty, 
                FilmTrajanje = projekcija.Film.Trajanje,
                FilmSlika = projekcija.Film.Slika ?? string.Empty, 
                DanId = projekcija.DanId,
                Datum = projekcija.Dan.Datum.ToDateTime(TimeOnly.MinValue), 
                TerminId = projekcija.TerminId,
                Vrijeme = projekcija.Termin.Vrijeme.ToTimeSpan(), 
                SalaId = projekcija.SalaId,
                SalaNaziv = projekcija.Sala.Naziv,
                Kapacitet = projekcija.Sala.Kapacitet,
                BrojSlobodnihMjesta = totalSeats - reservedSeats
            };

            return projekcijaViewModel;
        }

        

        [HttpGet("ByFilm/{filmId}/ByDate/{date}")]
        public async Task<ActionResult<IEnumerable<ProjekcijaViewModel>>> GetProjekcijeByFilmAndDate(int filmId, DateTime date)
        {
            var projekcije = await _context.Projekcije
                .Include(p => p.Film)
                .Include(p => p.Dan)
                .Include(p => p.Termin)
                .Include(p => p.Sala)
                .Where(p => p.FilmId == filmId && p.Dan.Datum == DateOnly.FromDateTime(date))
                .OrderBy(p => p.Termin.Vrijeme)
                .ToListAsync();

            var projekcijeViewModel = new List<ProjekcijaViewModel>();

            foreach (var projekcija in projekcije)
            {
                
                var totalSeats = await _context.Sjedista
                    .Where(s => s.SalaId == projekcija.SalaId)
                    .CountAsync();

                
                var reservedSeats = await _context.RezervisanaSjedista
                    .Include(rs => rs.Rezervacija)
                    .Where(rs => rs.Rezervacija.ProjekcijaId == projekcija.ProjekcijaId)
                    .CountAsync();

                DateOnly datum = projekcija.Dan.Datum;
                projekcijeViewModel.Add(new ProjekcijaViewModel
                {
                    ProjekcijaId = projekcija.ProjekcijaId,
                    FilmId = projekcija.FilmId,
                    FilmNaziv = projekcija.Film.Naziv,
                    FilmOpis = projekcija.Film.Opis,
                    FilmTrajanje = projekcija.Film.Trajanje,
                    FilmSlika = projekcija.Film.Slika,
                    DanId = projekcija.DanId,
                    Datum = datum.ToDateTime(TimeOnly.MinValue),
                    TerminId = projekcija.TerminId,
                    Vrijeme = projekcija.Termin.Vrijeme.ToTimeSpan(),
                    SalaId = projekcija.SalaId,
                    SalaNaziv = projekcija.Sala.Naziv,
                    Kapacitet = projekcija.Sala.Kapacitet,
                    BrojSlobodnihMjesta = totalSeats - reservedSeats
                });
            }

            return projekcijeViewModel;
        }
    }
}
