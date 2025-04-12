// Services/IEventService.cs
using EventEase.Models;

namespace EventEase.Services
{
    public interface IEventService
    {
        List<Event> GetEvents();
        void AddEvent(Event newEvent);
        void AddUser(Event registeredEvent, User newUser);
        Event GetEventById(int eventId);
        // Add this method to IEventService.cs
        bool IsUserRegistered(Event registeredEvent, User user);
    }
}