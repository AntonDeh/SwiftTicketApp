namespace SwiftTicketApp.Models
{
    public class ServiceHistory
    {
        public int ServiceHistoryId { get; set; } // Unique identifier for the service history record
        public int TicketId { get; set; } // Reference to the associated ticket
        public DateTime Date { get; set; } // The date of the service event
        public string ActionTaken { get; set; } = string.Empty; // Description of the action taken or change made
        public int UserId { get; set; }  // Identifier of the user who performed the action

        // Navigation properties
        public Ticket Ticket { get; set; } 
        public User User { get; set; }
    }
}
