


internal class Program
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
            Console.WriteLine("3. Borrow a book");
            Console.WriteLine("4. Return a book");
            Console.WriteLine("5. View all books");
            Console.WriteLine("6. Search for a book");
            Console.WriteLine("7. Exit");
            string? action = Console.ReadLine();

            switch (action)
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    RemoveBook();
                    break;
                case "3":
                    BorrowBook();
                    break;
                case "4":
                    ReturnBook();
                    break;
                case "5":
                    ViewBooks();
                    break;
                case "6":
                    Book? book = SearchBooks();
                    CheckSearchedBook(book);
                    break;
                case "7":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid input, please enter a valid input by entering a number betweeen 1 and 7");
                    break;

            }
        }

        void AddBook()
        {
            Console.Clear();
            Console.WriteLine("Please enter a book name");
            string? name = Console.ReadLine();
            if(name == null) Console.WriteLine("You must enter a book name to add a book.");
            else {
                Book book1 = new Book { Title = name, IsBorrowed = false };

                Books.Add(book1);
                Console.WriteLine($"{book1.Title} was added to the list of books.");
            }
            Close();
        }

        void RemoveBook()
        {
            Console.Clear();
            Book? book1 = SearchBooks();
            if(book1 == null) Console.WriteLine("Book not found, please check your spelling and make sure the book exists in the catalog before trying to remove.");
            else {
                Books.Remove(book1);
                Console.WriteLine($"{book1.Title} was removed from the list of books.");
            }
            Close();
        }

        void ViewBooks()
        {
            Console.Clear();
            Console.WriteLine("List of All Books In Catalog:");
            Console.WriteLine("============================");

            for(int i = 0; i < Books.Count; i++)
            {
                string checkedText = Books[i].IsBorrowed ? "[Checked Out]" : "[Available]";
                Console.WriteLine($"{Books[i].Title} is {checkedText}");
            }
            Close();
        }

        void ReturnBook()
        {
            Console.Clear();
            Book? book = SearchBooks();
            if(book == null || !book.IsBorrowed) Console.WriteLine("Book not returnable, verify the book you are trying to return exists in the catalog and has been checked out.");
            else {
                book.IsBorrowed = false;
                Console.WriteLine($"You have successfully returned {book.Title}");
            }
            Close();
        }

        void BorrowBook()
        {
            Console.Clear();
            if(CheckedOutBookCount() >= 3) {
                Console.WriteLine("Max checkout limit reached. You cannot borrow more books when you already have 3 books borrowed.");
                Close();
                return;
            }
            
            Book? book = SearchBooks();
            if(book == null || book.IsBorrowed) Console.WriteLine("Book cannot be checked out, verify the book you are trying to return exists in the catalog and is available to check out");
            else {
                book.IsBorrowed = true;
                Console.WriteLine($"You have successfully checked out {book.Title}");
            }
            Close();
        }
        
        void CheckSearchedBook(Book? book)
        {
            if(book == null) Console.WriteLine("Book not found in catalog.");
            else if(book.IsBorrowed == true) Console.WriteLine($"{book.Title} was found but it is already checked out");
            else Console.WriteLine($"{book.Title} was found and is available");
            Close();
        }

        Book? SearchBooks()
        {
            Console.Clear();
            Console.WriteLine("Please enter a book name");
            string? name = Console.ReadLine();

            Book? book1 = Books.Find(book => book.Title == name);
            return book1;
        }

        int CheckedOutBookCount()
        {
            return Books.Where(book => book.IsBorrowed == true).Count();
        }

        void Close()
        {
            Console.WriteLine("\nPress any key to return to menu.");
            Console.ReadKey();
        }
    }
}

class Book {
    public string? Title { get; set; }
    public bool IsBorrowed { get; set; }
}