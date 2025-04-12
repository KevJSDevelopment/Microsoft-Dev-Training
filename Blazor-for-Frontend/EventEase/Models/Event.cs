using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Event
    {
        public Event()
        {
            RegisteredUsers = new List<User>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string Location { get; set; }
        public List<User> RegisteredUsers { get; set; } 
    }
}