using RezervacijeBioskopskihKarata.Models;
using System;
using System.Collections.Generic;

namespace RezervacijeBioskopskihKarata.Models
{
    public class RezervacijaViewModel
    {
        public int RezervacijaId { get; set; }
        public int KorisnikId { get; set; }
        public int ProjekcijaId { get; set; }
        public DateTime DatumRezervacije { get; set; }
        
        
        public string NazivFilma { get; set; }
        public string Vrijeme { get; set; }
        public string Datum { get; set; }
        public string Sala { get; set; }
        public string KorisnikIme { get; set; }
        public string KorisnikPrezime { get; set; }
        public string KorisnikEmail { get; set; }
        
        
        public List<RezervisanoSjedisteViewModel> RezervisanaSjedista { get; set; }
    }

    public class RezervisanoSjedisteViewModel
    {
        public int Id { get; set; }
        public int SjedisteId { get; set; }
        public int RezervacijaId { get; set; }
        public int ProjekcijaId { get; set; }
        public int? BrojSjedista { get; set; }
        public string Red { get; set; }
    }

    public class RezervacijaCreateModel
    {
        public int KorisnikId { get; set; }
        public int ProjekcijaId { get; set; }
        public List<int> SjedistaIds { get; set; }
    }
}
