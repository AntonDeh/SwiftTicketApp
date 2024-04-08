namespace SwiftTicketApp.ViewModels.Tickets
{
    public class TicketDetailsViewModel
    {
        public int TicketId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
