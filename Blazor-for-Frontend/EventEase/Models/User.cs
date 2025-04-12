using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public class User
{
    public int Id { get; set; }
    [RegularExpression(@"^[a-zA-Z]+$"), Required]
    public string Name { get; set; }
    [Required, EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; }
}