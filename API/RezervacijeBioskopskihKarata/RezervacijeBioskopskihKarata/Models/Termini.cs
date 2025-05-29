using System;
using System.Collections.Generic;

namespace RezervacijeBioskopskihKarata.Models;

public partial class Termini
{
    public int TerminId { get; set; }

    public TimeOnly Vrijeme { get; set; }

    public virtual ICollection<Projekcije> Projekcijes { get; set; } = new List<Projekcije>();
}
