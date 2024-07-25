using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Custom.Database.Data;

public partial class DataEntities : DbContext
{
    public DataEntities(DbContextOptions<DataEntities> options)
        : base(options)
    {
    }

    public virtual DbSet<Pizza> Pizza { get; set; }

    public virtual DbSet<Topping> Topping { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pizza>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Topping>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.FK_PizzaNavigation).WithMany(p => p.Topping)
                .HasForeignKey(d => d.FK_Pizza)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Topping_Pizza_FK1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
