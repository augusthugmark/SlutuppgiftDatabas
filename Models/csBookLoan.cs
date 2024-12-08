using System;

namespace Library_Database_Program_Models;

public class BookLoan
{
    public int BookLoanID { get; set; } // PK
    public required DateTime LoanDate { get; set; } 
    public DateTime? ReturnDate { get; set; } 

    public required int BookID { get; set; } // FK till Book
    public Book Book { get; set; } // Navigation property för EF (inte en Composite Key)
}
