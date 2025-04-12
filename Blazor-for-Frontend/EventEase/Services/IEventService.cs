// Services/IEventService.cs
using EventEase.Models;

namespace EventEase.Services
{
    public interface IEventService
    {
        List<Event> GetEvents();
        void AddEvent(Event newEvent);
        // Add other methods as needed (GetEventById, UpdateEvent, DeleteEvent)
    }
}