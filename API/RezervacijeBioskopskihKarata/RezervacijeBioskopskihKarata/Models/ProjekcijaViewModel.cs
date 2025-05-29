using System;

namespace RezervacijeBioskopskihKarata.Models
{
    public class ProjekcijaViewModel
    {
        public int ProjekcijaId { get; set; }
        public int FilmId { get; set; }
        public string FilmNaziv { get; set; }
        public string FilmOpis { get; set; }
        public int FilmTrajanje { get; set; } 
        public string FilmSlika { get; set; }
        public int DanId { get; set; }
        public DateTime Datum { get; set; }
        public int TerminId { get; set; }
        public TimeSpan Vrijeme { get; set; }
        public int SalaId { get; set; }
        public string SalaNaziv { get; set; }
        public int Kapacitet { get; set; }
        public int BrojSlobodnihMjesta { get; set; }
    }
}
