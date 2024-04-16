using Microsoft.AspNetCore.Identity;

namespace SwiftTicketApp.Models
{
    public class User : IdentityUser
    { 
        // Navigation properties
        public ICollection<Ticket> TicketsCreated { get; set; } // Collection of tickets created by the user
        public ICollection<Comment> Comments { get; set; } // Collection of comments made by the user
        public ICollection<ServiceHistory> ServiceHistories { get; set; } // Collection of service history records associated with the user
        public User()
        {
            TicketsCreated = new HashSet<Ticket>();
            Comments = new HashSet<Comment>();
            ServiceHistories = new HashSet<ServiceHistory>();

        }
    }
}
