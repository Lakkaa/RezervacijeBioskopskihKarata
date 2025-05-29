using System;
using System.Collections.Generic;

namespace RezervacijeBioskopskihKarata.Models;

public partial class RezervisanaSjedista
{
    public int Id { get; set; }

    public int RezervacijaId { get; set; }

    public int ProjekcijaId { get; set; }

    public int SjedisteId { get; set; }

    public virtual Projekcije Projekcija { get; set; } = null!;

    public virtual Rezervacije Rezervacija { get; set; } = null!;

    public virtual Sjedista Sjediste { get; set; } = null!;
}
