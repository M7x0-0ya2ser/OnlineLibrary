using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models;

namespace OnlineLibrary.Data;

public partial class OnlineLibraryContext : DbContext
{
    public OnlineLibraryContext()
    {
    }

    public OnlineLibraryContext(DbContextOptions<OnlineLibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BorrowedBook> BorrowedBooks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-MFRG6F8; Database=OnlineLibrary; Trusted_Connection=True; MultipleActiveResultSets=True; TrustServerCertificate=True; encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK__Books__99F9D0A5EF2B0217");

            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("isbn");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Racknumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("racknumber");
            entity.Property(e => e.Stocknumber).HasColumnName("stocknumber");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<BorrowedBook>(entity =>
        {
            entity.HasKey(e => e.Ordernumber).HasName("PK__Borrowed__E866CE2BE38B6DA1");

            entity.Property(e => e.Ordernumber)
                .ValueGeneratedNever()
                .HasColumnName("ordernumber");
            entity.Property(e => e.Bookisbn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bookisbn");
            entity.Property(e => e.Dateofreturn).HasColumnName("dateofreturn");
            entity.Property(e => e.Isaccepted).HasColumnName("isaccepted");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.BookisbnNavigation).WithMany()
                .HasForeignKey(d => d.Bookisbn)
                .HasConstraintName("FK__BorrowedB__booki__300424B4");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("FK__BorrowedB__useri__30F848ED");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83FDBE2EA36");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Isaccepted).HasColumnName("isaccepted");
            entity.Property(e => e.Isadmin).HasColumnName("isadmin");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(1000)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Passwordsalt)
                .HasMaxLength(1000)
                .HasColumnName("passwordsalt");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
