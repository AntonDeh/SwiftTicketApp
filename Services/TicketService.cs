using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwiftTicketApp.Data;
using SwiftTicketApp.Interfaces;
using SwiftTicketApp.Models;
using SwiftTicketApp.ViewModels.Tickets;

namespace SwiftTicketApp.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse> CreateTicketAsync(CreateTicketViewModel model, string userId)
        {
            var serviceResponse = new ServiceResponse();

            try
            {
                var ticket = new Ticket
                {
                    UserId = userId,
                    CurrentSite = model.CurrentSite,
                    Category = model.Category,
                    Description = model.Description,
                    MobileNumber = model.MobileNumber,
                    LabLocation = model.LabLocation,
                    Urgency = model.Urgency,
  
                };

                // logic for processing and saving files if they were attached

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                serviceResponse.Success = true;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                // Handle exception
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }

        // Methods to fetch dropdown data
        public async Task<List<SelectListItem>> GetAvailableSitesAsync()
        {
            return await _context.Sites.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetAvailableCategoriesAsync()
        {
            return await _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetAvailableUrgenciesAsync()
        {
            return await _context.UrgencyLevels.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Name
            }).ToListAsync();
        }
        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId)
        {
            return await _context.Tickets
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

    }

    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

}
