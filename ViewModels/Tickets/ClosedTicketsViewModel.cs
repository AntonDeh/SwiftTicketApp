using SwiftTicketApp.Models;

namespace SwiftTicketApp.ViewModels.Tickets
{
    public class ClosedTicketsViewModel
    {
        public IEnumerable<Ticket> ClosedTickets { get; set; } = Enumerable.Empty<Ticket>();
    }
}
