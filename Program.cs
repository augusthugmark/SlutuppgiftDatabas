using System;
using Library_Database_Program_Seed;
using Library_Database_Program_Data;
using Library_Database_Program;

Console.WriteLine("Starting the database seeding process...");

try
{
    using (var context = new AppDbContext())
    {
        SeedData.Seed(context);
        Console.WriteLine("Seed data has been executed successfully.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while trying to seed the database: {ex.Message}");
}

var menu = new Menu();
menu.ShowMenu();
