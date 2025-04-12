// Services/EventService.cs
using EventEase.Models;

namespace EventEase.Services
{
    public class EventService : IEventService
    {
        private List<Event> _events;

        public EventService()
        {
            // Initialize with sample data
            _events = new List<Event>
            {
                new Event { Id = 1, Name = "DC Cherry Blossom Festival", StartDate = new DateTime(2024, 04, 01), Location = "Washington DC" },
                new Event { Id = 2, Name = "Coachella", StartDate = new DateTime(2024, 04, 11), Location = "Indio California" }
            };
        }

        public List<Event> GetEvents()
        {
            return _events;
        }

        public void AddEvent(Event newEvent)
        {
            // Auto-assign an ID if not provided
            if (newEvent.Id <= 0)
            {
                newEvent.Id = _events.Count > 0 ? _events.Max(e => e.Id) + 1 : 1;
            }
            
            _events.Add(newEvent);
        }
        
        public void AddUser(Event registeredEvent, User newUser)
        {
            if (IsUserRegistered(registeredEvent, newUser))
            {
                throw new InvalidOperationException("You are already registered for this event.");
            }
    
            registeredEvent.RegisteredUsers.Add(newUser);
        }

        public Event GetEventById(int eventId)
        {
            return _events.FirstOrDefault(e => e.Id == eventId);
        }
        
        // Add this method to EventService.cs
        public bool IsUserRegistered(Event registeredEvent, User user)
        {
            return registeredEvent.RegisteredUsers.Any(u => 
                u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));
        }
    }
}