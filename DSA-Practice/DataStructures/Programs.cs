namespace DataStructures;

public class Program
{
    static void BasicListMethod()
    {
        
    }

    static void LinkedListMethod()
    {
        LinkedList<string> sortedCards = new LinkedList<string>();

        sortedCards.AddLast("Jack of Clubs");
        sortedCards.AddLast("Queen of Diamonds");
        sortedCards.AddLast("King of Hearts");
        sortedCards.AddLast("Ace of Spades");

        Console.WriteLine("\nSorted Card Deck:");

        foreach(string card in sortedCards)
        {
            Console.WriteLine(card);
        }

        sortedCards.Remove("Jack of Clubs");

        Console.WriteLine("\nUpdated Sorted Card Deck:");
        foreach(string card in sortedCards)
        {
            Console.WriteLine(card);
        }
    }
}
