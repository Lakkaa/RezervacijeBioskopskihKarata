using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RezervacijeBioskopskihKarata.Models;

public partial class RezervacijeBioskopskihKarataContext : DbContext
{
    public RezervacijeBioskopskihKarataContext()
    {
    }

    public RezervacijeBioskopskihKarataContext(DbContextOptions<RezervacijeBioskopskihKarataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dani> Dani { get; set; }

    public virtual DbSet<Filmovi> Filmovi { get; set; }

    public virtual DbSet<Korisnici> Korisnici { get; set; }

    public virtual DbSet<Projekcije> Projekcije { get; set; }

    public virtual DbSet<Rezervacije> Rezervacije { get; set; }

    public virtual DbSet<RezervisanaSjedista> RezervisanaSjedista { get; set; }

    public virtual DbSet<Sale> Sale { get; set; }

    public virtual DbSet<Sjedista> Sjedista { get; set; }

    public virtual DbSet<Termini> Termini { get; set; }

  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dani>(entity =>
        {
            entity.HasKey(e => e.DanId).HasName("PK__dani__A23206423F9699B8");

            entity.ToTable("dani");

            entity.HasIndex(e => e.Datum, "UQ__dani__BA940B4D71D7BFF7").IsUnique();

            entity.Property(e => e.DanId).HasColumnName("dan_id");
            entity.Property(e => e.Datum).HasColumnName("datum");
        });

        modelBuilder.Entity<Filmovi>(entity =>
        {
            entity.HasKey(e => e.FilmId).HasName("PK__filmovi__349764A9B43DD7FF");

            entity.ToTable("filmovi");

            entity.Property(e => e.FilmId).HasColumnName("film_id");
            entity.Property(e => e.Naziv)
                .HasMaxLength(255)
                .HasColumnName("naziv");
            entity.Property(e => e.Opis).HasColumnName("opis");
            entity.Property(e => e.Slika)
                .HasMaxLength(255)
                .HasColumnName("slika");
            entity.Property(e => e.Trajanje).HasColumnName("trajanje");
        });

        modelBuilder.Entity<Korisnici>(entity =>
        {
            entity.HasKey(e => e.KorisnikId).HasName("PK__korisnic__B84D9F5683532779");

            entity.ToTable("korisnici");

            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");
            entity.Property(e => e.DatumRegistracije)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("datum_registracije");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Ime)
                .HasMaxLength(100)
                .HasColumnName("ime");
            entity.Property(e => e.Lozinka)
                .HasMaxLength(255)
                .HasColumnName("lozinka");
            entity.Property(e => e.Prezime)
                .HasMaxLength(100)
                .HasColumnName("prezime");
        });

        modelBuilder.Entity<Projekcije>(entity =>
        {
            entity.HasKey(e => e.ProjekcijaId).HasName("PK__projekci__9ADA420653C8E8EF");

            entity.ToTable("projekcije");

            entity.Property(e => e.ProjekcijaId).HasColumnName("projekcija_id");
            entity.Property(e => e.DanId).HasColumnName("dan_id");
            entity.Property(e => e.FilmId).HasColumnName("film_id");
            entity.Property(e => e.SalaId).HasColumnName("sala_id");
            entity.Property(e => e.TerminId).HasColumnName("termin_id");

            entity.HasOne(d => d.Dan).WithMany(p => p.Projekcijes)
                .HasForeignKey(d => d.DanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__projekcij__dan_i__48CFD27E");

            entity.HasOne(d => d.Film).WithMany(p => p.Projekcijes)
                .HasForeignKey(d => d.FilmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__projekcij__film___46E78A0C");

            entity.HasOne(d => d.Sala).WithMany(p => p.Projekcijes)
                .HasForeignKey(d => d.SalaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__projekcij__sala___47DBAE45");

            entity.HasOne(d => d.Termin).WithMany(p => p.Projekcijes)
                .HasForeignKey(d => d.TerminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__projekcij__termi__49C3F6B7");
        });

        modelBuilder.Entity<Rezervacije>(entity =>
        {
            entity.HasKey(e => e.RezervacijaId).HasName("PK__rezervac__A1EBD708FCEE5C01");

            entity.ToTable("rezervacije");

            entity.Property(e => e.RezervacijaId).HasColumnName("rezervacija_id");
            entity.Property(e => e.DatumRezervacije)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("datum_rezervacije");
            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");
            entity.Property(e => e.ProjekcijaId).HasColumnName("projekcija_id");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Rezervacijes)
                .HasForeignKey(d => d.KorisnikId)
                .HasConstraintName("FK__rezervaci__koris__4D94879B");

            entity.HasOne(d => d.Projekcija).WithMany(p => p.Rezervacijes)
                .HasForeignKey(d => d.ProjekcijaId)
                .HasConstraintName("FK__rezervaci__proje__4E88ABD4");
        });

        modelBuilder.Entity<RezervisanaSjedista>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rezervis__3213E83F3E87825E");

            entity.ToTable("rezervisana_sjedista");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProjekcijaId).HasColumnName("projekcija_id");
            entity.Property(e => e.RezervacijaId).HasColumnName("rezervacija_id");
            entity.Property(e => e.SjedisteId).HasColumnName("sjediste_id");

            entity.HasOne(d => d.Projekcija).WithMany(p => p.RezervisanaSjedista)
                .HasForeignKey(d => d.ProjekcijaId)
                .HasConstraintName("FK__rezervisa__proje__52593CB8");

            entity.HasOne(d => d.Rezervacija).WithMany(p => p.RezervisanaSjedista)
                .HasForeignKey(d => d.RezervacijaId)
                .HasConstraintName("FK__rezervisa__rezer__5165187F");

            entity.HasOne(d => d.Sjediste).WithMany(p => p.RezervisanaSjedista)
                .HasForeignKey(d => d.SjedisteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__rezervisa__sjedi__534D60F1");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SalaId).HasName("PK__sale__FA758FC7D36D4102");

            entity.ToTable("sale");

            entity.Property(e => e.SalaId).HasColumnName("sala_id");
            entity.Property(e => e.Kapacitet).HasColumnName("kapacitet");
            entity.Property(e => e.Naziv)
                .HasMaxLength(50)
                .HasColumnName("naziv");
            entity.Property(e => e.Slika)
                .HasMaxLength(255)
                .HasColumnName("slika");
        });

        modelBuilder.Entity<Sjedista>(entity =>
        {
            entity.HasKey(e => e.SjedisteId).HasName("PK__sjedista__D273433721EB9288");

            entity.ToTable("sjedista");

            entity.Property(e => e.SjedisteId).HasColumnName("sjediste_id");
            entity.Property(e => e.BrojSjedista).HasColumnName("broj_sjedista");
            entity.Property(e => e.Red)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("red");
            entity.Property(e => e.SalaId).HasColumnName("sala_id");

            entity.HasOne(d => d.Sala).WithMany(p => p.Sjedista)
                .HasForeignKey(d => d.SalaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__sjedista__sala_i__3F466844");
        });

        modelBuilder.Entity<Termini>(entity =>
        {
            entity.HasKey(e => e.TerminId).HasName("PK__termini__714C62A8B81B148E");

            entity.ToTable("termini");

            entity.Property(e => e.TerminId).HasColumnName("termin_id");
            entity.Property(e => e.Vrijeme).HasColumnName("vrijeme");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
