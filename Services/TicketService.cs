using SwiftTicketApp.Data;
using SwiftTicketApp.Interfaces;
using SwiftTicketApp.Models;
using SwiftTicketApp.ViewModels.Tickets;

namespace SwiftTicketApp.Services
{
    public class TicketService : ITicketService
    {
        // This example uses the database context for Entity Framework Core
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse> CreateTicketAsync(CreateTicketViewModel model)
        {
            var serviceResponse = new ServiceResponse();

            try
            {
                var ticket = new Ticket
                {

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

        // Methods for other ticket operations
    }

    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

}
