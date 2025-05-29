namespace RezervacijeBioskopskihKarata.Models
{
    public class FilmViewModel
    {
        public int ProjekcijaId { get; set; }
        public string? FilmNaziv { get; set; }
        public string? SalaNaziv { get; set; }
        public string? DanNaziv { get; set; }
        public string? TerminNaziv { get; set; }

        public int? FilmId { get; set; }

    }
}
