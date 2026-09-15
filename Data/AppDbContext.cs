using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication20260914.Data.Entities;

namespace WebApplication20260914.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<PromotionParticipant> PromotionParticipants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");

            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FamilyId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UnitCost).HasDefaultValue(1f, "DF_Item_UnitCost");
            entity.Property(e => e.UnitVol).HasDefaultValue(1f, "DF_Item_UnitVol");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.ToTable("Promotion");

            entity.Property(e => e.PromotionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PromotionParticipant>(entity =>
        {
            entity.HasKey(e => new { e.PromotionId, e.ItemId, e.CustomerId });

            entity.ToTable("PromotionParticipant");

            entity.Property(e => e.PromotionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
