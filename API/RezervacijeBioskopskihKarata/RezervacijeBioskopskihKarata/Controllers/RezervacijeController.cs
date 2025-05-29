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
    public class RezervacijeController : ControllerBase
    {
        private readonly RezervacijeBioskopskihKarataContext _context;

        public RezervacijeController(RezervacijeBioskopskihKarataContext context)
        {
            _context = context;
        }

        // GET: api/Rezervacije
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rezervacije>>> GetRezervacije()
        {
            return await _context.Rezervacije.ToListAsync();
        }

        // GET: api/Rezervacije/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rezervacije>> GetRezervacije(int id)
        {
            var rezervacije = await _context.Rezervacije.FindAsync(id);

            if (rezervacije == null)
            {
                return NotFound();
            }

            return rezervacije;
        }

        // PUT: api/Rezervacije/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRezervacije(int id, Rezervacije rezervacije)
        {
            if (id != rezervacije.RezervacijaId)
            {
                return BadRequest();
            }

            _context.Entry(rezervacije).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RezervacijeExists(id))
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

        // POST: api/Rezervacije
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rezervacije>> PostRezervacije(Rezervacije rezervacije)
        {
            _context.Rezervacije.Add(rezervacije);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRezervacije", new { id = rezervacije.RezervacijaId }, rezervacije);
        }

        // DELETE: api/Rezervacije/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRezervacije(int id)
        {
            var rezervacije = await _context.Rezervacije.FindAsync(id);
            if (rezervacije == null)
            {
                return NotFound();
            }

            _context.Rezervacije.Remove(rezervacije);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RezervacijeExists(int id)
        {
            return _context.Rezervacije.Any(e => e.RezervacijaId == id);
        }

        // GET: api/Rezervacije/Details/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult<RezervacijaViewModel>> GetRezervacijaDetails(int id)
        {
            var rezervacija = await _context.Rezervacije
                .Include(r => r.Projekcija)
                    .ThenInclude(p => p.Film)
                .Include(r => r.Projekcija.Termin)
                .Include(r => r.Projekcija.Dan)
                .Include(r => r.Projekcija.Sala)
                .Include(r => r.Korisnik)
                .Include(r => r.RezervisanaSjedista)
                    .ThenInclude(rs => rs.Sjediste)
                .FirstOrDefaultAsync(r => r.RezervacijaId == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            var rezervacijaViewModel = new RezervacijaViewModel
            {
                RezervacijaId = rezervacija.RezervacijaId,
                KorisnikId = (int)rezervacija.KorisnikId,
                ProjekcijaId = (int)rezervacija.ProjekcijaId,
                DatumRezervacije = (DateTime)rezervacija.DatumRezervacije,
                NazivFilma = rezervacija.Projekcija.Film.Naziv,
                Vrijeme = rezervacija.Projekcija.Termin.Vrijeme.ToString(),
                Datum = rezervacija.Projekcija.Dan.Datum.ToString("yyyy-MM-dd"),
                Sala = rezervacija.Projekcija.Sala.Naziv,
                KorisnikIme = rezervacija.Korisnik.Ime,
                KorisnikPrezime = rezervacija.Korisnik.Prezime,
                KorisnikEmail = rezervacija.Korisnik.Email,
                RezervisanaSjedista = rezervacija.RezervisanaSjedista.Select(rs => new RezervisanoSjedisteViewModel
                {
                    Id = rs.Id,
                    RezervacijaId = rs.RezervacijaId,
                    ProjekcijaId = rs.ProjekcijaId,
                    SjedisteId = rs.SjedisteId,
                    BrojSjedista = rs.Sjediste.BrojSjedista,
                    Red = rs.Sjediste.Red
                }).ToList()
            };

            return rezervacijaViewModel;
        }

        // POST: api/Rezervacije/CreateWithSeats
        [HttpPost("CreateWithSeats")]
        public async Task<ActionResult<RezervacijaViewModel>> CreateRezervacijaWithSeats(RezervacijaCreateModel model)
        {
            if (model.SjedistaIds == null || !model.SjedistaIds.Any())
            {
                return BadRequest("Sjedista trebaju biti odabrana");
            }

            
            var rezervacija = new Rezervacije
            {
                KorisnikId = model.KorisnikId,
                ProjekcijaId = model.ProjekcijaId,
                DatumRezervacije = DateTime.Now
            };

            _context.Rezervacije.Add(rezervacija);
            await _context.SaveChangesAsync();

            
            foreach (var sjedisteId in model.SjedistaIds)
            {
                var rezervisanoSjediste = new RezervisanaSjedista
                {
                    RezervacijaId = rezervacija.RezervacijaId,
                    ProjekcijaId = model.ProjekcijaId,
                    SjedisteId = sjedisteId
                };
                _context.RezervisanaSjedista.Add(rezervisanoSjediste);
            }

            await _context.SaveChangesAsync();

            
            return await GetRezervacijaDetails(rezervacija.RezervacijaId);
        }

       
        [HttpGet("ByProjekcija/{projekcijaId}")]
        public async Task<ActionResult<IEnumerable<RezervacijaViewModel>>> GetRezervacijeByProjekcija(int projekcijaId)
        {
            var rezervacije = await _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.RezervisanaSjedista)
                    .ThenInclude(rs => rs.Sjediste)
                .Where(r => r.ProjekcijaId == projekcijaId)
                .ToListAsync();

            var rezervacijeViewModel = rezervacije.Select(r => new RezervacijaViewModel
            {
                RezervacijaId = r.RezervacijaId,
                KorisnikId = r.KorisnikId ?? 0, 
                ProjekcijaId = r.ProjekcijaId ?? 0, 
                DatumRezervacije = r.DatumRezervacije ?? DateTime.MinValue,
                KorisnikIme = r.Korisnik?.Ime ?? string.Empty, 
                KorisnikPrezime = r.Korisnik?.Prezime ?? string.Empty, 
                KorisnikEmail = r.Korisnik?.Email ?? string.Empty, 
                RezervisanaSjedista = r.RezervisanaSjedista.Select(rs => new RezervisanoSjedisteViewModel
                {
                    BrojSjedista = rs.Sjediste?.BrojSjedista ?? 0, 
                    SjedisteId = rs.SjedisteId,
                    RezervacijaId = rs.RezervacijaId,
                    Red = rs.Sjediste?.Red ?? string.Empty 
                }).ToList()
            }).ToList();

            return rezervacijeViewModel;
        }
    }
}
