public class User
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

    public bool Validate()
    {
        if (string.IsNullOrWhiteSpace(Name) || Name.Length < 2)
            return false;
        
        if (!Email.Contains("@") || !Email.Contains("."))
            return false;

        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
            return false;

        return true;
    }
}
