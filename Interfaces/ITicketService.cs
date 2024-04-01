using SwiftTicketApp.Services;
using SwiftTicketApp.ViewModels.Tickets;

namespace SwiftTicketApp.Interfaces
{
    public interface ITicketService 
    {
        Task<ServiceResponse> CreateTicketAsync(CreateTicketViewModel model);
    }
}
