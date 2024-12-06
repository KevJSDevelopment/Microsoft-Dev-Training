internal partial class Program
{
    private static List<Product> Inventory = new List<Product>();
    private static void Main(string[] args)
    {
        bool running = true;
        while(running) {
            Console.Clear();
            ShowMenu();
            string? input = Console.ReadLine();

            switch(input)
            {
                case "1":
                    ViewAllProducts();
                    break;
                case "2":
                    AddProduct();
                    break;
                case "3":
                    UpdateStock();
                    break;
                case "4":
                    RemoveProduct();
                    break;
                case "5":
                    running = false;
                    break;
                default: 
                    Console.WriteLine("Invalid input, please enter a value 1-5. Press any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }
        
    }

    public static void ShowMenu() 
    {
        Console.WriteLine("=== Inventory Management System ===");
        Console.WriteLine("1. View Products");
        Console.WriteLine("2. Add Product");
        Console.WriteLine("3. Update Stock");
        Console.WriteLine("4. Remove Product");
        Console.WriteLine("5. Exit");
        Console.Write("Enter your choice: ");
    }

    static void AddProduct()
    {
        Console.WriteLine("Please enter the product name, price, and stock quantity");
        Console.WriteLine("Name: ");
        string? name = Console.ReadLine();
        while(Inventory.Any(product => product.Name == name)){
            Console.WriteLine("This product already exists, please enter another product name or enter 'exit' to exit the prompt");
            name = Console.ReadLine();
        }

        if(name == "exit" || name == null) return;

        Console.WriteLine("Price: ");
        decimal price;
        while(!decimal.TryParse(Console.ReadLine(), out price) || price < 0) Console.WriteLine("You must enter a price greater than 0. Enter a valid price:");
        Console.WriteLine("Quantity: ");
        int quantity;
        while(!int.TryParse(Console.ReadLine(), out quantity) || quantity < 0) Console.WriteLine("You must enter a number greater than 0. Enter a valid quantity:");
        
        Inventory.Add(new Product { Name = name, Price = price, StockQuantity = quantity });

        Console.WriteLine($"{name} was added to the inventory, press any key to return to the menu");
        Console.ReadKey();
    }

    static void RemoveProduct()
    {
        Console.WriteLine("Please enter the product name, price, and stock quantity");
        Console.WriteLine("Name: ");
        string? name = Console.ReadLine();
        Product? product = Inventory.Find(product => product.Name == name);
        while(product == null){
            Console.WriteLine("This product name you entered does not exist, please enter a product name or enter 'exit' to exit the prompt");
            ViewAllProducts();
            name = Console.ReadLine();
            if(name == "exit" || name == null) return;
            product = Inventory.Find(product => product.Name == name);
        }

        Inventory.Remove(product);
        Console.WriteLine($"{name} was removed from the inventory, press any key to return to the menu");
        Console.ReadKey();
    }

    static void UpdateStock()
    {
        Console.WriteLine("Please enter the product name, price, and stock quantity");
        Console.WriteLine("Name: ");
        string? name = Console.ReadLine();
        Product? product = Inventory.Find(product => product.Name == name);
        while(product == null){
            Console.WriteLine("This product name you entered does not exist, please enter a product name or enter 'exit' to exit the prompt");
            ViewAllProducts();
            name = Console.ReadLine();
            if(name == "exit" || name == null) return;
            product = Inventory.Find(product => product.Name == name);
        }

        Console.WriteLine($"How much is in stock for {product.Name}?");
        int quantity;
        while(!int.TryParse(Console.ReadLine(), out quantity) || quantity < 0) Console.WriteLine("You must enter a number greater than 0. Enter a valid quantity:");
        product.StockQuantity = quantity;
        Console.WriteLine($"{name} stock quantity was updated to {quantity}, press any key to return to the menu");
        Console.ReadKey();
    }

    static void ViewAllProducts()
    {   
        Console.WriteLine("\nCurrent Inventory:");
        Console.WriteLine("Name\t\tPrice\tQuantity");
        Console.WriteLine("----------------------------------------");
        
        foreach (var product in Inventory)
        {
            Console.WriteLine($"{product.Name,-15}\t${product.Price}\t{product.StockQuantity}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}

public class Product 
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}
