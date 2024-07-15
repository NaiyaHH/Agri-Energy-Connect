using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect.Models;

public partial class Poe2Context : DbContext
{
    public Poe2Context()
    {
    }

    public Poe2Context(DbContextOptions<Poe2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=NAIYAH;Initial Catalog=POE2;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2B71AED6D6");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryImg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__PRODUCT__B40CC6EDB3A2A63E");

            entity.ToTable("PRODUCT");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProductImg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PRODUCT__Categor__3E52440B");

            entity.HasOne(d => d.User).WithMany(p => p.Products)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PRODUCT__UserID__3F466844");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.UserRole).HasName("PK__Roles__81D4C6F1A2205598");

            entity.Property(e => e.UserRole)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__1788CCAC836ADC11");

            entity.ToTable("UserAccount");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserRole)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.UserAccounts)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAccou__UserR__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
