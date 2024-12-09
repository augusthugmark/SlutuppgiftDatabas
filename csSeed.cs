using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Library_Database_Program_Models;
using Library_Database_Program_Data;

namespace Library_Database_Program_Seed
{
    public static class SeedData
    {
        public static void Seed(AppDbContext context)
        {
        if (context.Authors.Any() && context.Books.Any())
            {
                Console.WriteLine("Authors and Books already exist. Proceeding with BookLoans only.");
            }

            SeedAuthorsAndBooks(context);
            SeedBookLoans(context);
        }

        // Seedar författare och böcker om databasen är fullständigt tom.
        private static void SeedAuthorsAndBooks(AppDbContext context)
        {
            if (context.Authors.Any() && context.Books.Any())
            {
                Console.WriteLine("Skipping seed of Authors and Books as data already exists.");
                return;
            }

            var author1 = new Author { FirstName = "Mary", LastName = "Shelley", DateOfBirth = new DateTime(1797, 8, 30) };
            var author2 = new Author { FirstName = "Isaac", LastName = "Asimov", DateOfBirth = new DateTime(1920, 1, 2) };
            var author3 = new Author { FirstName = "H.P.", LastName = "Lovecraft", DateOfBirth = new DateTime(1890, 8, 20) };
            var author4 = new Author { FirstName = "Gordon", LastName = "Ramsay", DateOfBirth = new DateTime(1966, 11, 8) };
            var author5 = new Author { FirstName = "Mark", LastName = "Twain", DateOfBirth = new DateTime(1835, 11, 30) };
            var author6 = new Author { FirstName = "Terry", LastName = "Pratchett", DateOfBirth = new DateTime(1948, 4, 28) };
            var author7 = new Author { FirstName = "Neil", LastName = "Gaiman", DateOfBirth = new DateTime(1964, 11, 10) };

            context.Authors.AddRange(author1, author2, author3, author4, author5, author6, author7);
            context.SaveChanges();

            var book1 = new Book { Title = "Frankenstein", PublicationYear = 1818, Genre = "Horror" };
            var book2 = new Book { Title = "Foundation", PublicationYear = 1951, Genre = "Sci-Fi" };
            var book3 = new Book { Title = "The Call of Cthulhu", PublicationYear = 1928, Genre = "Horror" };
            var book4 = new Book { Title = "Hell's Kitchen Cookbook", PublicationYear = 2015, Genre = "Cooking" };
            var book5 = new Book { Title = "The Adventures of Tom Sawyer", PublicationYear = 1876, Genre = "Horror" };
            var book6 = new Book { Title = "Twain's Cooking", PublicationYear = 1880, Genre = "Cooking" };
            var book7 = new Book { Title = "Good Omens", PublicationYear = 1990, Genre = "Fantasy" };

            context.Books.AddRange(book1, book2, book3, book4, book5, book6, book7);
            context.SaveChanges();

            context.BookAuthorLinks.AddRange(
                new BookAuthorLink { BookID = book1.BookID, AuthorID = author1.AuthorID },
                new BookAuthorLink { BookID = book2.BookID, AuthorID = author2.AuthorID },
                new BookAuthorLink { BookID = book3.BookID, AuthorID = author3.AuthorID },
                new BookAuthorLink { BookID = book5.BookID, AuthorID = author5.AuthorID },
                new BookAuthorLink { BookID = book6.BookID, AuthorID = author5.AuthorID },
                new BookAuthorLink { BookID = book4.BookID, AuthorID = author4.AuthorID },
                new BookAuthorLink { BookID = book7.BookID, AuthorID = author6.AuthorID },
                new BookAuthorLink { BookID = book7.BookID, AuthorID = author7.AuthorID }
            );

            context.SaveChanges();
            Console.WriteLine("Authors and Books have been successfully seeded.");
        }

        private static void SeedBookLoans(AppDbContext context)
        {
            if (context.BookLoans.Any())
            {
                Console.WriteLine("BookLoans already exist. Skipping seeding.");
                return;
            }

            var baseLoanedDate = new DateTime(2024, 12, 1);

            var books = context.Books.ToList();
            if (!books.Any())
            {
                Console.WriteLine("No books found to seed BookLoans.");
                return;
            }

            var bookLoans = new List<BookLoan>();

            for (int i = 0; i < books.Count; i++)
            {
                var loanedDate = baseLoanedDate.AddDays(i);
                var returnDate = loanedDate.AddDays(30 + i);

                bookLoans.Add(new BookLoan
                {
                    BookID = books[i].BookID,
                    LoanDate = loanedDate,
                    ReturnDate = returnDate
                });
            }

            context.BookLoans.AddRange(bookLoans);
            context.SaveChanges();
            Console.WriteLine($"Seeded {bookLoans.Count} BookLoans.");
        }
    }
}



