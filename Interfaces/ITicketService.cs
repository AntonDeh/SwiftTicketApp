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
        Task<ServiceResponse> CloseTicketAsync(int ticketId, string userId);
        Task<IEnumerable<Ticket>> SearchTicketsAsync(string searchTerm);
        Task<IEnumerable<Ticket>> GetClosedTicketsByUserIdAsync(string userId);
        Task<IEnumerable<Ticket>> GetAllClosedTicketsAsync();
        Task<IEnumerable<Ticket>> GetFilteredTicketsAsync(string status, string submitter, string subCategory, string technician, string urgencyLevel, string site);
        Task<ServiceResponse> UpdateTicketStatusAsync(int ticketId, string userId, string newStatus);
        Task<ServiceResponse> AssignTicketToTechnicianAsync(int ticketId, string userId);
        Task<ServiceResponse> AddCommentAsync(int ticketId, string userId, string content);
        Task<List<SelectListItem>> GetAvailableStatusesAsync();
        Task<List<SelectListItem>> GetAvailableSubmittersAsync();
        Task<List<SelectListItem>> GetAvailableTechniciansAsync();
        Task<List<SelectListItem>> GetTicketsWithTechnicianNameAsync();
        Task<List<CommentViewModel>> GetCommentsByTicketIdAsync(int ticketId);
        Task<List<SelectListItem>> GetUsersForDropdownAsync();


    }
}
