using System;
using System.Collections.Generic;

namespace RezervacijeBioskopskihKarata.Models;

public partial class Sjedista
{
    public int SjedisteId { get; set; }

    public int SalaId { get; set; }

    public int BrojSjedista { get; set; }

    public string Red { get; set; } = null!;

    public virtual ICollection<RezervisanaSjedista> RezervisanaSjedista { get; set; } = new List<RezervisanaSjedista>();

    public virtual Sale Sala { get; set; } = null!;
}
