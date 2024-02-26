namespace SwiftTicketApp.Models
{
    public class Ticket
    {
        public int TicketId { get; set; } // Unique identifier for the ticket
        public string Title { get; set; } = string.Empty; // The title of the ticket
        public string Description { get; set; } = string.Empty; // Detailed description of the ticket issue
        public DateTime CreatedAt { get; set; } // The date and time the ticket was created
        public DateTime? ClosedAt { get; set; } // The date and time the ticket was closed, nullable if still open
        public string Status { get; set; } = string.Empty; // The current status of the ticket (e.g., "Open", "In Progress", "Closed")
        public required string UserId { get; set; } // Reference to the user who created the ticket

        // Navigation properties
        public User? User { get; set; } // The user who created the ticket
        public ICollection<Comment> Comments { get; set; } // Collection of comments associated with the ticket
        public ICollection<ServiceHistory> ServiceHistories { get; set; } // Collection of service history records for the ticket

        public Ticket()
        {
            Comments = new HashSet<Comment>();
            ServiceHistories = new HashSet<ServiceHistory>();
        }
    }
}
