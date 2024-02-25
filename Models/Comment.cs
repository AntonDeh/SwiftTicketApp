﻿namespace SwiftTicketApp.Models
{
    public class Comment
    {
        public int CommentId { get; set; } // Unique identifier for the comment
        public int TicketId { get; set; } // Reference to the associated ticket
        public required string UserId { get; set; } // Identifier of the user who left the comment
        public required string Content { get; set; } // The text of the comment
        public DateTime CreatedAt { get; set; } // The date and time the comment was created

        // Navigation properties
        public required Ticket Ticket { get; set; }
        public required User User { get; set; }
    }
}
