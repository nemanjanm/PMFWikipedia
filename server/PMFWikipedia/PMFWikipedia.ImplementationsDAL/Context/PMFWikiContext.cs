﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public partial class PMFWikiContext : DbContext
    {
        public PMFWikiContext()
        {
        }

        public PMFWikiContext(DbContextOptions<PMFWikiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<FavoriteSubject> FavoriteSubjects { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chat");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.HasOne(d => d.User1Navigation)
                    .WithMany(p => p.ChatUser1Navigations)
                    .HasForeignKey(d => d.User1)
                    .HasConstraintName("FK__Chat__User1__0A9D95DB");

                entity.HasOne(d => d.User2Navigation)
                    .WithMany(p => p.ChatUser2Navigations)
                    .HasForeignKey(d => d.User2)
                    .HasConstraintName("FK__Chat__User2__0B91BA14");
            });

            modelBuilder.Entity<FavoriteSubject>(entity =>
            {
                entity.ToTable("FavoriteSubject");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.FavoriteSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteSubject_SubjectId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteSubjects)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteSubject_UserId");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.Content).IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ChatId)
                    .HasConstraintName("FK__Message__ChatId__0E6E26BF");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Content).IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.PostAuthorNavigations)
                    .HasForeignKey(d => d.Author)
                    .HasConstraintName("FK__Post__Author__160F4887");

                entity.HasOne(d => d.LastEditedByNavigation)
                    .WithMany(p => p.PostLastEditedByNavigations)
                    .HasForeignKey(d => d.LastEditedBy)
                    .HasConstraintName("FK__Post__LastEdited__17036CC0");

                entity.HasOne(d => d.SubjectNavigation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.Subject)
                    .HasConstraintName("FK__Post__Subject__17F790F9");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.ConnectionId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ConnectionID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoPath)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegisterToken)
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
}