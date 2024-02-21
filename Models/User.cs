namespace SwiftTicketApp.Models
{
    public class User
    {
        public int UserId { get; set; } // Unique identifier for the user
        public string Username { get; set; } = string.Empty; // User's username
        public string Email { get; set; } = string.Empty; // User's email address
        public string PasswordHash { get; set; } = string.Empty; // Hash of the user's password for secure storage
        public string Role { get; set; } = string.Empty; // User's role in the system (e.g., "Admin", "Technician", "Client")

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
