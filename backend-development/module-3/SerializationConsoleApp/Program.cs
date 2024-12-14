using System.Text.Json;
using System.IO;
using System.Xml.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        
        Person samplePerson = new Person { UserName = "test", UserAge = 21};

        //1. Binary serialization
        using (FileStream fs = new FileStream("person.dat", FileMode.Create))
        {
            BinaryWriter writer = new BinaryWriter(fs);
            writer.Write(samplePerson.UserName);
            writer.Write(samplePerson.UserAge);
        }

        Console.WriteLine("Binary serialization complete");

        //2. XML serialization
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Person));

        using (StreamWriter writer = new StreamWriter("person.xml"))
        {
            xmlSerializer.Serialize(writer, samplePerson);
        }

        //3. JSON serialization
        string jsonString = JsonSerializer.Serialize(samplePerson);

        File.WriteAllText("person.json", jsonString);
        Console.WriteLine("JSON serialization complete.");
    }
}