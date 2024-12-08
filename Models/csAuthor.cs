using System;
using System.Collections.Generic;

namespace Library_Database_Program_Models;

public class Author
{
    public int AuthorID { get; set; } // PK
    public required string FirstName { get; set; } 
    public required string LastName { get; set; } 
    public required DateTime DateOfBirth { get; set; } 

    public ICollection<BookAuthorLink> BookAuthorLinks { get; set; } = new List<BookAuthorLink>();
}
