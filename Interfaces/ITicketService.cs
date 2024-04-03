using Microsoft.AspNetCore.Mvc.Rendering;
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
    }
}
