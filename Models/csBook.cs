using System;
using System.Collections.Generic;

namespace Library_Database_Program_Models;

public class Book
{
    public int BookID { get; set; } // PK
    public required string Title { get; set; } 
    public required int PublicationYear { get; set; } 
    public required string Genre { get; set; } 

    public ICollection<BookAuthorLink> BookAuthorLinks { get; set; } = new List<BookAuthorLink>();
    public ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
}
