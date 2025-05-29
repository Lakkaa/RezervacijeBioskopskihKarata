using System;
using System.Collections.Generic;

namespace RezervacijeBioskopskihKarata.Models;

public partial class Rezervacije
{
    public int RezervacijaId { get; set; }

    public int? KorisnikId { get; set; }

    public int? ProjekcijaId { get; set; }

    public DateTime? DatumRezervacije { get; set; }

    public virtual Korisnici? Korisnik { get; set; }

    public virtual Projekcije? Projekcija { get; set; }

    public virtual ICollection<RezervisanaSjedista> RezervisanaSjedista { get; set; } = new List<RezervisanaSjedista>();
}
