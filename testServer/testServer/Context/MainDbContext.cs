using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using testServer.Models;

namespace testServer.Context;

public partial class MainDbContext : DbContext
{
    public MainDbContext()
    {
    }

    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CwiczenieBlok> CwiczenieBloks { get; set; }

    public virtual DbSet<CwiczenieUzytkownika> CwiczenieUzytkownikas { get; set; }

    public virtual DbSet<ExternalAuth> ExternalAuths { get; set; }

    public virtual DbSet<LocalAuth> LocalAuths { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<Trening> Trenings { get; set; }

    public virtual DbSet<Uzytkownik> Uzytkowniks { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CwiczenieBlok>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cwiczenie_blok_pk");

            entity.ToTable("cwiczenie_blok");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CwiczenieUzytkownikaId).HasColumnName("cwiczenie_uzytkownika_id");
            entity.Property(e => e.CwiczenieWger).HasColumnName("cwiczenie_wger");
            entity.Property(e => e.SetId).HasColumnName("set_id");
            entity.Property(e => e.TreningId).HasColumnName("trening_id");

            entity.HasOne(d => d.CwiczenieUzytkownika).WithMany(p => p.CwiczenieBloks)
                .HasForeignKey(d => d.CwiczenieUzytkownikaId)
                .HasConstraintName("cwiczenie_blok_cwiczenie_uzytkownika");

            entity.HasOne(d => d.Set).WithMany(p => p.CwiczenieBloks)
                .HasForeignKey(d => d.SetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cwiczenie_blok_set");

            entity.HasOne(d => d.Trening).WithMany(p => p.CwiczenieBloks)
                .HasForeignKey(d => d.TreningId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cwiczenie_blok_trening");
        });

        modelBuilder.Entity<CwiczenieUzytkownika>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cwiczenie_uzytkownika_pk");

            entity.ToTable("cwiczenie_uzytkownika");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nazwa");
            entity.Property(e => e.Opis)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("opis");
            entity.Property(e => e.UzytkownikId).HasColumnName("uzytkownik_id");

            entity.HasOne(d => d.Uzytkownik).WithMany(p => p.CwiczenieUzytkownikas)
                .HasForeignKey(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cwiczenie_uzytkownika_uzytkownik");
        });

        modelBuilder.Entity<ExternalAuth>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("external_auth_pk");

            entity.ToTable("external_auth");

            entity.HasIndex(e => new { e.Dostawca, e.IdOdDostawcy }, "UQ_ExternalAuth_Provider_ProviderUserId").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dostawca)
                .HasMaxLength(50)
                .HasColumnName("dostawca");
            entity.Property(e => e.IdOdDostawcy)
                .HasMaxLength(255)
                .HasColumnName("id_od_dostawcy");
            entity.Property(e => e.UzytkownikId).HasColumnName("uzytkownik_id");

            entity.HasOne(d => d.Uzytkownik).WithMany(p => p.ExternalAuths)
                .HasForeignKey(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("external_auth_uzytkownik");
        });

        modelBuilder.Entity<LocalAuth>(entity =>
        {
            entity.HasKey(e => e.UzytkownikId).HasName("local_auth_pk");

            entity.ToTable("local_auth");

            entity.Property(e => e.UzytkownikId)
                .ValueGeneratedNever()
                .HasColumnName("uzytkownik_id");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(512)
                .HasColumnName("password_hash");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(512)
                .HasColumnName("password_salt");

            entity.HasOne(d => d.Uzytkownik).WithOne(p => p.LocalAuth)
                .HasForeignKey<LocalAuth>(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("local_auth_uzytkownik");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plan_pk");

            entity.ToTable("plan");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nazwa");
            entity.Property(e => e.UzytkownikId).HasColumnName("uzytkownik_id");

            entity.HasOne(d => d.Uzytkownik).WithMany(p => p.Plans)
                .HasForeignKey(d => d.UzytkownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_uzytkownik");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("set_pk");

            entity.ToTable("set");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ciezar).HasColumnName("ciezar");
            entity.Property(e => e.IloscPowtorzen).HasColumnName("ilosc_powtorzen");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("test");

            entity.Property(e => e.A).HasColumnName("a");
        });

        modelBuilder.Entity<Trening>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trening_pk");

            entity.ToTable("trening");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");

            entity.HasOne(d => d.Plan).WithMany(p => p.Trenings)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("trening_plan");
        });

        modelBuilder.Entity<Uzytkownik>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uzytkownik_pk");

            entity.ToTable("uzytkownik");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
