using EventEase.Models;
namespace EventEase.Services;

public class UserService : IUserService
{
    private List<User> _users;

    public UserService()
    {
        _users = new List<User>();
    }

    public List<User> GetUsers()
    {
        return _users;
    }
}