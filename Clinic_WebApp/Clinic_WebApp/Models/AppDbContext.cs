using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Clinic_WebApp.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Consultation> Consultations { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasKey(e => e.ConsultId).HasName("PRIMARY");

            entity.ToTable("consultations");

            entity.HasIndex(e => e.DocId, "docID");

            entity.HasIndex(e => e.PatId, "patID");

            entity.Property(e => e.ConsultId)
                .HasColumnType("int(11)")
                .HasColumnName("consultID");
            entity.Property(e => e.ConsultDate)
                .HasColumnType("datetime")
                .HasColumnName("consultDate");
            entity.Property(e => e.Diagnosis)
                .HasColumnType("text")
                .HasColumnName("diagnosis");
            entity.Property(e => e.DocId)
                .HasColumnType("int(11)")
                .HasColumnName("docID");
            entity.Property(e => e.PatId)
                .HasColumnType("int(11)")
                .HasColumnName("patID");
            entity.Property(e => e.Prescription)
                .HasColumnType("text")
                .HasColumnName("prescription");

            entity.HasOne(d => d.Doc).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.DocId)
                .HasConstraintName("consultations_ibfk_2");

            entity.HasOne(d => d.Pat).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.PatId)
                .HasConstraintName("consultations_ibfk_1");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DocId).HasName("PRIMARY");

            entity.ToTable("doctors");

            entity.Property(e => e.DocId)
                .HasColumnType("int(11)")
                .HasColumnName("docID");
            entity.Property(e => e.DocAddress)
                .HasColumnType("text")
                .HasColumnName("docAddress");
            entity.Property(e => e.DocFname)
                .HasMaxLength(100)
                .HasColumnName("docFName");
            entity.Property(e => e.DocLname)
                .HasMaxLength(100)
                .HasColumnName("docLName");
            entity.Property(e => e.DocSpecialization)
                .HasMaxLength(100)
                .HasColumnName("docSpecialization");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatId).HasName("PRIMARY");

            entity.ToTable("patients");

            entity.Property(e => e.PatId)
                .HasColumnType("int(11)")
                .HasColumnName("patID");
            entity.Property(e => e.PatBirthDate).HasColumnName("patBirthDate");
            entity.Property(e => e.PatFname)
                .HasMaxLength(100)
                .HasColumnName("patFName");
            entity.Property(e => e.PatLname)
                .HasMaxLength(100)
                .HasColumnName("patLName");
            entity.Property(e => e.PatTelNo)
                .HasMaxLength(20)
                .HasColumnName("patTelNo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
