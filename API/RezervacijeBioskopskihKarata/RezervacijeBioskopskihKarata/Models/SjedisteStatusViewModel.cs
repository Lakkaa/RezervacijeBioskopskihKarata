namespace RezervacijeBioskopskihKarata.Models
{
    public class SjedisteStatusViewModel
    {
        public int SjedisteId { get; set; }
        public int SalaId { get; set; }
        public int BrojSjedista { get; set; }
        public string Red { get; set; }
        public bool Zauzeto { get; set; }
    }
}
