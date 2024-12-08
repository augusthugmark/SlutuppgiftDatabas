using Microsoft.EntityFrameworkCore;
using Library_Database_Program_Models;

namespace Library_Database_Program_Data;

public class AppDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; } 
    public DbSet<Book> Books { get; set; } 
    public DbSet<BookLoan> BookLoans { get; set; } 
    public DbSet<BookAuthorLink> BookAuthorLinks { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-7L0MNJ0\\SQLEXPRESS02;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    // Denna kod hanterar relationer och sammansatta nycklar mellan tabeller
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Sammansatt nyckel är en kombination av BookID och AuthorID som identifierar varje relation.
        modelBuilder.Entity<BookAuthorLink>()
            .HasKey(ba => new { ba.BookID, ba.AuthorID });

        // Kommenterar ut alla relationer här bara för att va extremt nogrann och se exakt vad varje rad kod gör.
        // En bok kan ha flera boklån, men ett boklån hör till en specifik bok.
        modelBuilder.Entity<BookLoan>()
            .HasOne(bl => bl.Book) // En BookLoan har en referens till en enskild Book.
            .WithMany(b => b.BookLoans) // En Book kan ha flera BookLoans.
            .HasForeignKey(bl => bl.BookID); // Använder BookID som främmande nyckel i BookLoan.

        // En bok kan vara kopplad till flera författare, och en författare kan vara kopplad till flera böcker.
        modelBuilder.Entity<BookAuthorLink>()
            .HasOne(ba => ba.Book) // En BookAuthorLink har en referens till en specifik Book.
            .WithMany(b => b.BookAuthorLinks) // En Book kan ha flera BookAuthorLinks.
            .HasForeignKey(ba => ba.BookID); // Använd BookID som främmande nyckel i BookAuthorLink.

        // Definierar relationer mellan BookAuthorLink och Author.
        modelBuilder.Entity<BookAuthorLink>()
            .HasOne(ba => ba.Author) // En BookAuthorLink har en referens till en specifik Author.
            .WithMany(a => a.BookAuthorLinks) // En Author kan vara kopplad till flera BookAuthorLinks.
            .HasForeignKey(ba => ba.AuthorID); // Använd AuthorID som främmande nyckel i BookAuthorLink.
    }
}
