using Microsoft.AspNetCore.Mvc.Rendering;
using SwiftTicketApp.Models;
using SwiftTicketApp.Services;
using SwiftTicketApp.ViewModels.Tickets;

namespace SwiftTicketApp.Interfaces
{
    public interface ITicketService 
    {
        Task<ServiceResponse> CreateTicketAsync(CreateTicketViewModel model, string userId);
        Task<List<SelectListItem>> GetAvailableSitesAsync();
        Task<List<SelectListItem>> GetAvailableCategoriesAsync();
        Task<List<SelectListItem>> GetAvailableUrgenciesAsync();
        Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId);
        Task<Ticket?> GetTicketByIdAsync(int ticketId);
        Task<bool> UpdateTicketAsync(EditTicketViewModel model);

    }
}
