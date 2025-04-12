using EventEase.Models;

namespace EventEase.Services;

public interface IUserService
{
    List<User> GetUsers();
}