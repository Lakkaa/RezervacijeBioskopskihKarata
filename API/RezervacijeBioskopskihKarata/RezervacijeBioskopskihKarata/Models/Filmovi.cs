using System;
using System.Collections.Generic;

namespace RezervacijeBioskopskihKarata.Models;

public partial class Filmovi
{
    public int FilmId { get; set; }

    public string Naziv { get; set; } = null!;

    public string? Opis { get; set; }

    public int Trajanje { get; set; }

    public string? Slika { get; set; }

    public virtual ICollection<Projekcije> Projekcijes { get; set; } = new List<Projekcije>();
}
