using System;

namespace Library_Database_Program_Models;

public class BookAuthorLink
{
    public required int BookID { get; set; } // FK till Book
    public Book Book { get; set; } // Ska användas till Composite Key (som ska hanteras i DbContext)

    public required int AuthorID { get; set; } // FK till Author
    public Author Author { get; set; } // Ska användas till Composite Key (som ska hanteras i DbContext)
}
