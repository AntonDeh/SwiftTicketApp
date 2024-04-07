using SwiftTicketApp.Models;

namespace SwiftTicketApp.ViewModels.Tickets
{
    public class TicketSearchViewModel
    {
        public string SearchTerm { get; set; } = string.Empty;
        public IEnumerable<Ticket> SearchResults { get; set; } = Enumerable.Empty<Ticket>();
    }
}
