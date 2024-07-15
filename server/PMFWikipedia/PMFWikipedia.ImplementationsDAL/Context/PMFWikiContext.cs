﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.Models;

namespace PMFWikipedia.ImplementationsDAL.PMFWikipedia.ImplementationsDAL;

public partial class PMFWikiContext : DbContext
{
    public PMFWikiContext(DbContextOptions<PMFWikiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FavoriteSubject> FavoriteSubjects { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FavoriteSubject>(entity =>
        {
            entity.ToTable("FavoriteSubject");

            entity.HasOne(d => d.Subject).WithMany(p => p.FavoriteSubjects)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteSubject_SubjectId");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteSubjects)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteSubject_UserId");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subject");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhotoPath)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RegisterToken)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RegisterTokenExpirationTime).HasColumnType("datetime");
            entity.Property(e => e.ResetToken)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResetTokenExpirationTime).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}