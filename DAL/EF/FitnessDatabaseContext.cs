using System;
using System.Collections.Generic;
using FitnessHere.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessHere.DAL.EF;

public partial class FitnessDatabaseContext : DbContext
{
    public FitnessDatabaseContext()
    {
    }

    public FitnessDatabaseContext(DbContextOptions<FitnessDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberClass> MemberClasses { get; set; }

    public virtual DbSet<MemberClassStatus> MemberClassStatuses { get; set; }

    public virtual DbSet<MembersLog> MembersLogs { get; set; }

    public virtual DbSet<Recurrence> Recurrences { get; set; }

    public virtual DbSet<Trainer> Trainers { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Classes__CB1927A0F6725319");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClassName).HasMaxLength(255);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RecurrenceId).HasColumnName("RecurrenceID");
            entity.Property(e => e.ScheduleDateTime).HasColumnType("datetime");
            entity.Property(e => e.TrainerId).HasColumnName("TrainerID");

            entity.HasOne(d => d.Recurrence).WithMany(p => p.Classes)
                .HasForeignKey(d => d.RecurrenceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Classes_Recurrence");

            entity.HasOne(d => d.Trainer).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TrainerId)
                .HasConstraintName("FK_Classes_Trainer");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Members__0CF04B3842549E2E");

            entity.HasIndex(e => e.Email, "UQ__Members__A9D10534B4230468").IsUnique();

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<MemberClass>(entity =>
        {
            entity.HasKey(e => e.MemberClassId).HasName("PK__MemberCl__DF21376BA1322F3C");

            entity.Property(e => e.MemberClassId).HasColumnName("MemberClassID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.JoinDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MemberClassStatusId).HasColumnName("MemberClassStatusID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.Class).WithMany(p => p.MemberClasses)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_MemberClasses_Class");

            entity.HasOne(d => d.MemberClassStatus).WithMany(p => p.MemberClasses)
                .HasForeignKey(d => d.MemberClassStatusId)
                .HasConstraintName("FK_MemberClasses_Status");

            entity.HasOne(d => d.Member).WithMany(p => p.MemberClasses)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_MemberClasses_Member");
        });

        modelBuilder.Entity<MemberClassStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__MemberCl__C8EE2043BADD9BB1");

            entity.ToTable("MemberClassStatus");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(255);
        });

        modelBuilder.Entity<MembersLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Members_Log");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.MemberId)
                .ValueGeneratedOnAdd()
                .HasColumnName("MemberID");
            entity.Property(e => e.Operation)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationUser).HasMaxLength(4000);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Recurrence>(entity =>
        {
            entity.HasKey(e => e.RecurrenceId).HasName("PK__Recurren__9D537B759A5EB48E");

            entity.Property(e => e.RecurrenceId).HasColumnName("RecurrenceID");
            entity.Property(e => e.RecurrenceType).HasMaxLength(255);
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.HasKey(e => e.TrainerId).HasName("PK__Trainers__366A1B9C121D25C8");

            entity.HasIndex(e => e.Email, "UQ__Trainers__A9D10534C7742DC7").IsUnique();

            entity.Property(e => e.TrainerId).HasColumnName("TrainerID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B5BE9A5EC");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Member).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_Transactions_Member");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
