﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftTicketApp.Models
{
    public class Ticket
    {
        public int TicketId { get; set; } // Unique identifier for the ticket
        public string Title { get; set; } = string.Empty; // The title of the ticket
        public string Description { get; set; } = string.Empty; // Detailed description of the ticket issue
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // The date and time the ticket was created
        public DateTime? ClosedAt { get; set; } // The date and time the ticket was closed, nullable if still open
        
        [ForeignKey("TicketStatus")]
        public int StatusId { get; set; } = 1; // Assigning the status "New" by default

        public virtual TicketStatus? TicketStatus { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; // Reference to the user who created the ticket

        public string CurrentSite { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string SubCategory { get; set; } = string.Empty;
        public string Urgency { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string LabLocation { get; set; } = string.Empty;

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
