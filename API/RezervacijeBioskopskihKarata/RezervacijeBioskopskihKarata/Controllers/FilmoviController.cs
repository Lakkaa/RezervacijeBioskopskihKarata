using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RezervacijeBioskopskihKarata.Models;
using System.IO;

namespace RezervacijeBioskopskihKarata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmoviController : ControllerBase
    {
        private readonly RezervacijeBioskopskihKarataContext _context;

        public FilmoviController(RezervacijeBioskopskihKarataContext context)
        {
            _context = context;
        }

        // GET: api/Filmovi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filmovi>>> GetFilmovi()
        {
            return await _context.Filmovi.ToListAsync();
        }

        // GET: api/Filmovi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Filmovi>> GetFilmovi(int id)
        {
            var filmovi = await _context.Filmovi.FindAsync(id);

            if (filmovi == null)
            {
                return NotFound();
            }

            return filmovi;
        }

        // PUT: api/Filmovi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilmovi(int id, Filmovi filmovi)
        {
            if (id != filmovi.FilmId)
            {
                return BadRequest();
            }

            _context.Entry(filmovi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmoviExists(id))
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

        // POST: api/Filmovi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Filmovi>> PostFilmovi(Filmovi filmovi)
        {
            _context.Filmovi.Add(filmovi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFilmovi", new { id = filmovi.FilmId }, filmovi);
        }

        // DELETE: api/Filmovi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilmovi(int id)
        {
            var filmovi = await _context.Filmovi.FindAsync(id);
            if (filmovi == null)
            {
                return NotFound();
            }

            _context.Filmovi.Remove(filmovi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FilmoviExists(int id)
        {
            return _context.Filmovi.Any(e => e.FilmId == id);
        }
    

        [HttpGet("projekcijeByFilm/{filmId}")]
        public async Task<ActionResult<IEnumerable<FilmViewModel>>> GetProjekcijeByFilm(int filmId)
        {
            var projekcije = await _context.Projekcije
                .Include(p => p.Film)
                .Include(p => p.Sala)
                .Include(p => p.Dan)
                .Include(p => p.Termin)
                .Where(p => p.FilmId == filmId)
                .ToListAsync();

            if (!projekcije.Any())
            {
                return NotFound();
            }

            var viewModels = projekcije.Select(projekcija => new FilmViewModel
            {
                ProjekcijaId = projekcija.ProjekcijaId,
                FilmId = projekcija.FilmId,
                FilmNaziv = projekcija.Film?.Naziv,
                SalaNaziv = projekcija.Sala?.Naziv,
                DanNaziv = projekcija.Dan?.Datum.ToString("yyyy-MM-dd"),
                TerminNaziv = projekcija.Termin?.Vrijeme.ToString("hh\\:mm")
            }).ToList();

            return viewModels;
        }


    }
    }
