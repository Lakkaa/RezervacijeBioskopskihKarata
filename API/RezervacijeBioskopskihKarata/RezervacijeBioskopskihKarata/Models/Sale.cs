using System;
using System.Collections.Generic;

namespace RezervacijeBioskopskihKarata.Models;

public partial class Sale
{
    public int SalaId { get; set; }

    public string Naziv { get; set; } = null!;

    public int Kapacitet { get; set; }

    public string? Slika { get; set; }

    public virtual ICollection<Projekcije> Projekcijes { get; set; } = new List<Projekcije>();

    public virtual ICollection<Sjedista> Sjedista { get; set; } = new List<Sjedista>();
}
