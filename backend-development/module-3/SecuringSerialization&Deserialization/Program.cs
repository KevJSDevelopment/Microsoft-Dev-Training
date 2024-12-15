using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

internal class Program
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string GenerateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(ToString()));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public void EncryptData()
        {
            Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(Password));
        }
    }

    public static string SerializeUserData(User user)
    {
        if(string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Name))
        {
            Console.WriteLine("Invalid Data. Serialization aborted.");
            return string.Empty;
        }

        user.EncryptData();
        return JsonSerializer.Serialize(user);
    }

    
    private static User DeserializeUserData(string serializedData, bool isTrustedSource)
    {
        if(!isTrustedSource)
        {
            Console.WriteLine("Deserialization blocked: untrusted source");
            return null;
        }

        return JsonSerializer.Deserialize<User>(serializedData);
    }

    private static void Main(string[] args)
    {
        User user = new User
        {
            Name = "Alice",
            Email = "alice@example.com",
            Password = "testP@ass123"
        };

        string generateHash = user.GenerateHash();
        string serializedData = SerializeUserData(user);
        User deserializeData = DeserializeUserData(serializedData, true);
        Console.WriteLine("Serialized Data: \n" + generateHash);
    }

}