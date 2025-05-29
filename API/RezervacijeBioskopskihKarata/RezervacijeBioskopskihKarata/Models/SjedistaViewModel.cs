namespace RezervacijeBioskopskihKarata.ViewModels
{
    public class SjedistaViewModel
    {
        public int SalaId { get; set; }
        public string? SalaNaziv { get; set; }
        public int? BrojSjedista { get; set; }
        public int? RezervisanoSjedisteId { get; set; }
        public int? ProjekcijaId { get; set; }
        public int? RezervacijaId { get; set; }
        public int? SjedisteId { get; set; }
        public string? Red { get; set; }
        public bool IsReserved { get; set; }
    }
}
