using Microsoft.AspNetCore.Identity;

namespace SwiftTicketApp.Models
{
    public class TicketAssignment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public required Ticket Ticket { get; set; }
        public string TechnicianId { get; set; } = string.Empty;
        public required IdentityUser Technician { get; set; }

        public TicketAssignment()
        {
            //
        }
    }
}
