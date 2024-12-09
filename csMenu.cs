using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Library_Database_Program_Data;
using Library_Database_Program_Models;

namespace Library_Database_Program
{
    public class Menu
    {
        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Library Management System");
                Console.WriteLine("1. Add a new author");
                Console.WriteLine("2. Add a new book");
                Console.WriteLine("3. Create a relationship between a book and an author");
                Console.WriteLine("4. Add a loan for a book");
                Console.WriteLine("5. Remove an author");
                Console.WriteLine("6. Remove a book");
                Console.WriteLine("7. Remove a loan");
                Console.WriteLine("8. List all books with their authors");
                Console.WriteLine("9. List all borrowed books with their return dates");
                Console.WriteLine("10. List all books by a specific author");
                Console.WriteLine("11. List all authors who contributed to a specific book");
                Console.WriteLine("12. Show loan history");
                Console.WriteLine("13. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddAuthor(); break;
                    case "2": AddBook(); break;
                    case "3": AddBookAuthorRelation(); break;
                    case "4": AddLoan(); break;
                    case "5": RemoveAuthor(); break;
                    case "6": RemoveBook(); break;
                    case "7": RemoveLoan(); break;
                    case "8": ListBooksWithAuthors(); break;
                    case "9": ListBorrowedBooks(); break;
                    case "10": ListBooksByAuthor(); break;
                    case "11": ListAuthorsForBook(); break;
                    case "12": ShowLoanHistory(); break;
                    case "13": Console.WriteLine("Exiting..."); return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }

                Console.WriteLine("\nPress Enter to return to the menu.");
                Console.ReadLine();
            }
        }

        private void AddAuthor()
        {
            Console.Write("Enter author's first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter author's last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter author's date of birth (yyyy-mm-dd): ");
            DateTime dateOfBirth = DateTime.Parse(Console.ReadLine());

            using (var context = new AppDbContext())
            {
                context.Authors.Add(new Author 
                { 
                    FirstName = firstName, 
                    LastName = lastName,
                    DateOfBirth = dateOfBirth 
                });
                context.SaveChanges();
                Console.WriteLine($"Author '{firstName} {lastName}' added.");
            }
        }


        private void AddBook()
        {
            try
            {
                Console.Write("Enter book title: ");
                string title = Console.ReadLine();
                Console.Write("Enter book genre: ");
                string genre = Console.ReadLine();
                
                int year;
                while (true)
                {
                    Console.Write("Enter publication year(example 1984): ");
                    if (int.TryParse(Console.ReadLine(), out year))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid year. Please enter a valid number.");
                }

                using (var context = new AppDbContext())
                {
                    context.Books.Add(new Book { Title = title, Genre = genre, PublicationYear = year });
                    context.SaveChanges();
                    Console.WriteLine($"Book '{title}' added.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void AddBookAuthorRelation()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    Console.Write("Enter book title: ");
                    var bookTitle = Console.ReadLine();

                    Console.Write("Enter author's full name: ");
                    var authorName = Console.ReadLine();

                    // Hitta den befintliga boken i databasen
                    var book = context.Books.SingleOrDefault(b => b.Title == bookTitle);

                    if (book == null)
                    {
                        Console.WriteLine($"No book found with the title '{bookTitle}'. Cannot proceed.");
                        return;
                    }

                    // Hitta den befintliga författaren i databasen
                    var author = context.Authors
                        .AsEnumerable()
                        .SingleOrDefault(a => $"{a.FirstName} {a.LastName}" == authorName);

                    if (author == null)
                    {
                        Console.WriteLine($"No author found with the name '{authorName}'. Cannot proceed.");
                        return;
                    }

                    // Skapa relation mellan bok och författare
                    var bookAuthorLink = new BookAuthorLink
                    {
                        BookID = book.BookID,
                        AuthorID = author.AuthorID
                    };

                    context.BookAuthorLinks.Add(bookAuthorLink);
                    context.SaveChanges();

                    Console.WriteLine($"Successfully linked '{bookTitle}' to '{authorName}'.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

        Console.WriteLine("Press Enter to return to the menu.");
        Console.ReadLine();

        }

        private void AddLoan()
        {
            try
            {
                Console.Write("Enter book title to loan: ");
                string bookTitle = Console.ReadLine();
                
                DateTime loanDate, returnDate;
                while (true)
                {
                    Console.Write("Enter loan date (yyyy-mm-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out loanDate))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid date. Please use the format yyyy-mm-dd.");
                }

                while (true)
                {
                    Console.Write("Enter return date (yyyy-mm-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out returnDate))
                    {
                        if (returnDate >= loanDate)
                        {
                            break;
                        }
                        Console.WriteLine("Return date must be after the loan date.");
                    }
                    Console.WriteLine("Invalid date. Please use the format yyyy-mm-dd.");
                }

                using (var context = new AppDbContext())
                {
                    var book = context.Books.FirstOrDefault(b => b.Title == bookTitle);
                    if (book != null)
                    {
                        context.BookLoans.Add(new BookLoan { BookID = book.BookID, LoanDate = loanDate, ReturnDate = returnDate });
                        context.SaveChanges();
                        Console.WriteLine($"Loan for book '{bookTitle}' created.");
                    }
                    else
                    {
                        Console.WriteLine("Book not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void RemoveAuthor()
        {
            Console.Write("Enter author's full name to remove: ");
            var authorName = Console.ReadLine();

            using (var context = new AppDbContext())
            {
                try
                {
                    // Sök efter författaren i minnet för att lösa strängjämförelse-problem
                    var author = context.Authors
                        .AsEnumerable()
                        .SingleOrDefault(a => $"{a.FirstName} {a.LastName}" == authorName);

                    if (author != null)
                    {
                        context.Authors.Remove(author);
                        context.SaveChanges();
                        Console.WriteLine($"Author '{authorName}' removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No author found with that name.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            
            Console.WriteLine("Press Enter to return to the menu.");
            Console.ReadLine();
        }

        private void RemoveBook()
        {
            Console.Write("Enter book title to remove: ");
            string title = Console.ReadLine();

            using (var context = new AppDbContext())
            {
                var book = context.Books.FirstOrDefault(b => b.Title == title);
                if (book != null)
                {
                    context.Books.Remove(book);
                    context.SaveChanges();
                    Console.WriteLine("Book removed.");
                }
            }
        }

        private void RemoveLoan()
        {
            Console.Write("Enter book title to remove loan for: ");
            string title = Console.ReadLine();

            using (var context = new AppDbContext())
            {
                var book = context.Books.FirstOrDefault(b => b.Title == title);

                if (book != null)
                {
                    var loan = context.BookLoans.FirstOrDefault(l => l.BookID == book.BookID);
                    if (loan != null)
                    {
                        context.BookLoans.Remove(loan);
                        context.SaveChanges();
                        Console.WriteLine("Loan removed.");
                    }
                    else
                    {
                        Console.WriteLine("No loan found for that book.");
                    }
                }
                else
                {
                    Console.WriteLine("Book not found.");
                }
            }
        }


        private void ListBooksWithAuthors()
        {
            using (var context = new AppDbContext())
            {
                var booksWithAuthors = (from link in context.BookAuthorLinks
                                        join book in context.Books on link.BookID equals book.BookID
                                        join author in context.Authors on link.AuthorID equals author.AuthorID
                                        select new
                                        {
                                            Book = book,
                                            Author = author
                                        }).ToList();

                Console.WriteLine("Books with their authors:");
                foreach (var item in booksWithAuthors)
                {
                    Console.WriteLine($"Book: {item.Book?.Title}, Author: {item.Author?.FirstName} {item.Author?.LastName}");
                }
            }
        }

        private void ListBorrowedBooks()
        {
            using (var context = new AppDbContext())
            {
                var borrowedBooks = (from loan in context.BookLoans
                                    join book in context.Books on loan.BookID equals book.BookID
                                    select new
                                    {
                                        BookTitle = book.Title,
                                        LoanedOn = loan.LoanDate,
                                        ReturnDate = loan.ReturnDate
                                    }).ToList();

                Console.WriteLine("Borrowed Books with their return dates:");
                foreach (var loan in borrowedBooks)
                {
               
                string loanedOnFormatted = loan.LoanedOn.ToString("yyyy-MM-dd");

                
                string returnDateFormatted = loan.ReturnDate.HasValue 
                    ? loan.ReturnDate.Value.ToString("yyyy-MM-dd") 
                    : "Not Returned Yet";  

                Console.WriteLine($"Book: {loan.BookTitle}, Loaned On: {loanedOnFormatted}, Return Date: {returnDateFormatted}");
                }
            }
        }

        private void ListBooksByAuthor()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                Console.Write("Enter author's full name: ");
                var authorName = Console.ReadLine();

                // Hitta författaren
                var author = context.Authors
                    .AsNoTracking()
                    .SingleOrDefault(a => (a.FirstName + " " + a.LastName) == authorName);

                if (author != null)
                {
                    Console.WriteLine($"Found author: {author.FirstName} {author.LastName}");

                    // Hitta alla böcker kopplade till författaren via BookAuthorLink
                    var books = context.BookAuthorLinks
                    .AsNoTracking()
                    .Where(link => link.AuthorID == author.AuthorID)
                    .Join(
                    context.Books,
                    link => link.BookID,
                    book => book.BookID,
                    (link, book) => book
                        )
                        .ToList();

                        if (books.Any())
                        {
                            Console.WriteLine("Books by this author:");
                            foreach (var book in books)
                            {
                                Console.WriteLine($"- {book.Title} (Genre: {book.Genre}, Year: {book.PublicationYear})");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No books found for this author.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Author not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine("Press Enter to return to the menu.");
                Console.ReadLine();
            }
        }

        private void ListAuthorsForBook()
        {
            Console.Write("Enter book title: ");
            string bookTitle = Console.ReadLine();

            using (var context = new AppDbContext())
            {
                var book = context.Books
                    .FirstOrDefault(b => b.Title == bookTitle);

                if (book != null)
                {
                    var authors = (from link in context.BookAuthorLinks
                                join author in context.Authors on link.AuthorID equals author.AuthorID
                                where link.BookID == book.BookID
                                select author).ToList();

                    Console.WriteLine($"Authors contributing to '{bookTitle}':");
                    foreach (var author in authors)
                    {
                        Console.WriteLine($"- {author?.FirstName} {author?.LastName}");
                    }
                }
                else
                {
                    Console.WriteLine("Book not found.");
                }
            }
        }

        private void ShowLoanHistory()
        {
            using (var context = new AppDbContext())
            {
                var loanHistory = (from loan in context.BookLoans
                                join book in context.Books on loan.BookID equals book.BookID
                                select new
                                {
                                    BookTitle = book.Title,
                                    LoanedOn = loan.LoanDate,
                                    ReturnDate = loan.ReturnDate
                                }).ToList();

                Console.WriteLine("Loan History:");

                if (loanHistory.Count == 0)
                {
                    Console.WriteLine("No books have been borrowed.");
                }
                else
                {
                    foreach (var loan in loanHistory)
                    {
                        string returnDateFormatted = loan.ReturnDate.HasValue 
                            ? loan.ReturnDate.Value.ToString("yyyy-MM-dd") 
                            : "Not Returned Yet";

                        Console.WriteLine($"Book: {loan.BookTitle}, Loaned On: {loan.LoanedOn:yyyy-MM-dd}, Return Date: {returnDateFormatted}");
                    }
                }
            }
        }

    }
}

