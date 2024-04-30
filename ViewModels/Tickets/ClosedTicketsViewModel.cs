using Microsoft.AspNetCore.Mvc.Rendering;
using SwiftTicketApp.Models;

namespace SwiftTicketApp.ViewModels.Tickets
{
    public class ClosedTicketsViewModel
    {
        public IEnumerable<Ticket> ClosedTickets { get; set; } = Enumerable.Empty<Ticket>();
        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();
        public string SelectedUserId { get; set; } = string.Empty;
    }
}
