using System;
using System.Collections.Generic;

namespace RezervacijeBioskopskihKarata.Models;

public partial class Korisnici
{
    public int KorisnikId { get; set; }

    public string Ime { get; set; } = null!;

    public string Prezime { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Lozinka { get; set; }

    public DateTime? DatumRegistracije { get; set; }

    public virtual ICollection<Rezervacije> Rezervacijes { get; set; } = new List<Rezervacije>();
}
