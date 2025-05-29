using System;
using System.Collections.Generic;

namespace RezervacijeBioskopskihKarata.Models;

public partial class Dani
{
    public int DanId { get; set; }

    public DateOnly Datum { get; set; }

    public virtual ICollection<Projekcije> Projekcijes { get; set; } = new List<Projekcije>();
}
