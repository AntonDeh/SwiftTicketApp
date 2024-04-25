namespace SwiftTicketApp.Models
{
    public class Comment
    {
        public int CommentId { get; set; } // Unique identifier for the comment
        public int TicketId { get; set; } // Reference to the associated ticket
        public string UserId { get; set; } = string.Empty;  // Identifier of the user who left the comment
        public string Content { get; set; } = string.Empty;  // The text of the comment
        public DateTime CreatedAt { get; set; } // The date and time the comment was created

        // Navigation properties
        public required Ticket Ticket { get; set; }
        public required User User { get; set; }
    }
}
