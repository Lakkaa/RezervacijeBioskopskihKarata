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
    public class KorisniciController : ControllerBase
    {
        private readonly RezervacijeBioskopskihKarataContext _context;

        public KorisniciController(RezervacijeBioskopskihKarataContext context)
        {
            _context = context;
        }

        // GET: api/Korisnicis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Korisnici>>> GetKorisnici()
        {
            return await _context.Korisnici.ToListAsync();
        }

        // GET: api/Korisnicis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Korisnici>> GetKorisnici(int id)
        {
            var korisnici = await _context.Korisnici.FindAsync(id);

            if (korisnici == null)
            {
                return NotFound();
            }

            return korisnici;
        }

        // GET: api/Korisnicis/ByEmail/{email}
        [HttpGet("ByEmail/{email}")]
        public async Task<ActionResult<Korisnici>> GetKorisnikByEmail(string email)
        {
            var korisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Email.ToLower() == email.ToLower());

            if (korisnik == null)
            {
                return NotFound();
            }

            return korisnik;
        }

        // PUT: api/Korisnicis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKorisnici(int id, Korisnici korisnici)
        {
            if (id != korisnici.KorisnikId)
            {
                return BadRequest();
            }

            _context.Entry(korisnici).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KorisniciExists(id))
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

        // POST: api/Korisnicis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Korisnici>> PostKorisnici(Korisnici korisnici)
        {
            
            var existingKorisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Email.ToLower() == korisnici.Email.ToLower());
            
            if (existingKorisnik != null)
            {
                return Conflict("Korisnik sa ovim mejlom vec postoji");
            }

            _context.Korisnici.Add(korisnici);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKorisnici", new { id = korisnici.KorisnikId }, korisnici);
        }

        // DELETE: api/Korisnicis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKorisnici(int id)
        {
            var korisnici = await _context.Korisnici.FindAsync(id);
            if (korisnici == null)
            {
                return NotFound();
            }

            _context.Korisnici.Remove(korisnici);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KorisniciExists(int id)
        {
            return _context.Korisnici.Any(e => e.KorisnikId == id);
        }
        // GET: api/Korisnicis/Rezervacije
        [HttpGet("Rezervacije")]
        public async Task<ActionResult<IEnumerable<object>>> KorisniciSaRezervacijama()
        {
            var korisniciWithRezervacije = await _context.Korisnici
                .Include(k => k.Rezervacijes)
                .Select(k => new
                {
                    k.KorisnikId,
                    k.Ime,
                    k.Prezime,
                    k.Email,
                    Rezervacije = k.Rezervacijes.Select(r => new
                    {
                        r.RezervacijaId,
                        r.ProjekcijaId,
                        r.DatumRezervacije
                    }).ToList()
                })
                .ToListAsync();

            return Ok(korisniciWithRezervacije);
        }
        // GET: api/Korisnicis/Rezervacije
        [HttpGet("Rezervacije/{id}")]
        public async Task<ActionResult<object>> KorisnikSaRezervacijama(int id)
        {
            var korisnikWithRezervacije = await _context.Korisnici
                .Where(k => k.KorisnikId == id)
                .Include(k => k.Rezervacijes)
                .Select(k => new
                {
                    k.KorisnikId,
                    k.Ime,
                    k.Prezime,
                    k.Email,
                    Rezervacije = k.Rezervacijes.Select(r => new
                    {
                        r.RezervacijaId,
                        r.ProjekcijaId,
                        r.DatumRezervacije
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (korisnikWithRezervacije == null)
            {
                return NotFound();
            }

            return Ok(korisnikWithRezervacije);
        }
        // GET: api/Korisnicis/Rezervacije/Details/{id}
        [HttpGet("Rezervacije/Details/{id}")]
        public async Task<ActionResult<object>> GetKorisnikRezervacijeDetails(int id)
        {
            var korisnikDetails = await _context.Korisnici.Where(k => k.KorisnikId == id).Include(k => k.Rezervacijes)
                .ThenInclude(r => r.Projekcija) .ThenInclude(p => p.Film)
                .Include(k => k.Rezervacijes) .ThenInclude(r => r.RezervisanaSjedista) .ThenInclude(rs => rs.Sjediste)
                .Include(k => k.Rezervacijes) .ThenInclude(r => r.Projekcija) .ThenInclude(p => p.Termin)
                .Select(k => new
                {
                    k.KorisnikId,
                    k.Ime,
                    k.Prezime,
                    k.Email,
                    Rezervacije = k.Rezervacijes.Select(r => new
                    {
                        r.RezervacijaId,
                        Film = r.Projekcija.Film.Naziv,
                        Termin = r.Projekcija.Termin.Vrijeme,
                        RezervisanaSjedista = r.RezervisanaSjedista.Select(rs => new
                        {
                            rs.Sjediste.Red,
                            rs.Sjediste.BrojSjedista
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (korisnikDetails == null)
            {
                return NotFound();
            }

            return Ok(korisnikDetails);
        }
    }




}

