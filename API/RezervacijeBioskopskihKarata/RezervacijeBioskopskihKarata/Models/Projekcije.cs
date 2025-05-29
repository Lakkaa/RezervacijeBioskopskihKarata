using System;
using System.Collections.Generic;

namespace RezervacijeBioskopskihKarata.Models;

public partial class Projekcije
{
    public int ProjekcijaId { get; set; }

    public int FilmId { get; set; }

    public int SalaId { get; set; }

    public int DanId { get; set; }

    public int TerminId { get; set; }

    public virtual Dani Dan { get; set; } = null!;

    public virtual Filmovi Film { get; set; } = null!;

    public virtual ICollection<Rezervacije> Rezervacijes { get; set; } = new List<Rezervacije>();

    public virtual ICollection<RezervisanaSjedista> RezervisanaSjedista { get; set; } = new List<RezervisanaSjedista>();

    public virtual Sale Sala { get; set; } = null!;

    public virtual Termini Termin { get; set; } = null!;
}
