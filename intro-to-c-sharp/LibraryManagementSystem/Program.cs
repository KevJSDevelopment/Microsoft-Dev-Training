﻿internal class Program
{
    private static List<Book> Books = new List<Book>();
    private static void Main(string[] args)
    {
        bool running = true;
        
        while (running)
        {
            Console.Clear();
            Books = Books.OrderBy(book => book.Title).ToList();
            Console.WriteLine("Would would you like to do?:");
            Console.WriteLine("1. Add a book");
            Console.WriteLine("2. Remove a book");
            Console.WriteLine("3. View all books");
            Console.WriteLine("4. Exit");
            string action = Console.ReadLine();

            switch (action)
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    RemoveBook();
                    break;
                case "3":
                    ViewBooks();
                    break;
                case "4":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid input, please enter a valid input by entering a number betweeen 1 and 4");
                    break;

            }
        }

        void AddBook()
        {
            Console.Clear();
            Console.WriteLine("Please enter a book name");
            string name = Console.ReadLine();

            Book book1 = new Book { Title = name };

            Books.Add(book1);
            Console.WriteLine($"{book1.Title} was added to the list of books. Press any key to return to menu.");
            Console.ReadKey();
        }

        void RemoveBook()
        {
            Console.Clear();
            Console.WriteLine("Please enter a book name");
            string name = Console.ReadLine();

            Book book1 = Books.Find(book => book.Title == name);
            if(book1 == null) Console.WriteLine("Book not found, please check your spelling and make sure the book exists in the catalog before trying to remove. Press any key to return to menu");
            else {
                Books.Remove(book1);
                Console.WriteLine($"{book1.Title} was removed from the list of books. Press any key to return to menu.");
            }
            Console.ReadKey();
        }

        void ViewBooks()
        {
            Console.Clear();
            Console.WriteLine("List of All Books Available:");
            Console.WriteLine("============================");

            for(int i = 0; i < Books.Count; i++)
            {
                Console.WriteLine($"{Books[i].Title}");
            }

            Console.WriteLine("\nPress any key to return to menu.");
            Console.ReadKey();
        }
    }
}

class Book {
    public string Title { get; set; }
}