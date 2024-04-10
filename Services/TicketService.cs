using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public TicketService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ServiceResponse> CreateTicketAsync(CreateTicketViewModel model, string userId)
        {
            var serviceResponse = new ServiceResponse();
            var defaultStatus = await _context.TicketStatuses.FirstOrDefaultAsync(s => s.Name == "New");
            if (defaultStatus == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Default status 'New' not found.";
                return serviceResponse;
            }
            try
            {
                var ticket = new Ticket
                {
                    UserId = userId,
                    StatusId = defaultStatus.Id,
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
            //  Find the "Closed" status ID in the database asynchronously.
            var closedStatusId = await _context.TicketStatuses
                                               .Where(s => s.Name == "Closed")
                                               .Select(s => s.Id)
                                               .FirstOrDefaultAsync();
            if (closedStatusId == 0)
            {
                // If the "Closed" status ID is not found, log the error or handle it accordingly.
                return new List<Ticket>();
            }

            // We return all user tickets that do not have the "Closed" status.
            return await _context.Tickets
                                 .Where(t => t.UserId == userId && t.StatusId != closedStatusId)
                                 .Include(t => t.TicketStatus)
                                 .ToListAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets
                .Include(t => t.TicketStatus)
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }


        public async Task<bool> UpdateTicketAsync(EditTicketViewModel model)
        {
            var ticket = await _context.Tickets.FindAsync(model.TicketId);
            if (ticket == null)
            {
                return false; // Ticket not found
            }

            ticket.Description = model.Description;
            ticket.CurrentSite = ticket.CurrentSite;
            ticket.Category = ticket.Category;
            ticket.MobileNumber = ticket.MobileNumber;
            ticket.LabLocation = ticket.LabLocation;
            ticket.Urgency = ticket.Urgency;

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return true; // Successful update
        }
        public async Task<ServiceResponse> CloseTicketAsync(int ticketId, string userId)
        {
            var serviceResponse = new ServiceResponse();
            // We look for a ticket by ID and check that the user has access to it
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticketId && t.UserId == userId);

            if (ticket == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Ticket not found or access denied.";
                return serviceResponse;
            }

            // We look for the "Closed" status in the status table
            var closedStatus = await _context.TicketStatuses.FirstOrDefaultAsync(s => s.Name == "Closed");
            if (closedStatus == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Status 'Closed' not found.";
                return serviceResponse;
            }

            // Assign the status ID "Closed" to the ticket
            ticket.StatusId = closedStatus.Id;

            try
            {
                _context.Tickets.Update(ticket);
                await _context.SaveChangesAsync();
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
        public async Task<IEnumerable<Ticket>> SearchTicketsAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Enumerable.Empty<Ticket>();
            }

            return await _context.Tickets
              .Where(t => t.Description.Contains(searchTerm) || t.TicketId.ToString() == searchTerm)
              .Include(t => t.TicketStatus)
              .ToListAsync();
        }
        public async Task<IEnumerable<Ticket>> GetClosedTicketsByUserIdAsync(string userId)
        {
            int closedStatusId = _context.TicketStatuses.FirstOrDefault(s => s.Name == "Closed")?.Id ?? 0;
            return await _context.Tickets
                .Where(t => t.UserId == userId && t.StatusId == closedStatusId)
                .Include(t => t.TicketStatus)
                .ToListAsync();
        }
        public async Task<List<SelectListItem>> GetAvailableStatusesAsync()
        {
            // Query to status table in database
            var statuses = await _context.TicketStatuses
                .Select(status => new SelectListItem
                {
                    Value = status.Name,
                    Text = status.Name
                })
                .ToListAsync();

            return statuses;
        }
        public async Task<List<SelectListItem>> GetAvailableSubmittersAsync()
        {
            // user table associated with tickets
            var submitters = await _context.Users
                .Select(user => new SelectListItem
                {
                    Value = user.UserName, 
                    Text = user.UserName
                })
                .Distinct() 
                .ToListAsync();

            return submitters;
        }
        public async Task<List<SelectListItem>> GetAvailableTechniciansAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("Technician");
            return users.Select(user => new SelectListItem 
            { 
                Value = user.Id, 
                Text = user.UserName 
            })
            .ToList();
        }
        public async Task<List<SelectListItem>> GetTicketsWithTechnicianNameAsync()
        {
            var ticketsWithTechnicianName = await _context.TicketAssignments
                .Include(ta => ta.Ticket)
                .Include(ta => ta.Technician)
                .Select(ta => new SelectListItem
                {
                    Value = ta.TicketId.ToString(),
                    Text = $"{ta.Ticket.Description} - {ta.Technician.UserName}" 
                })
                .ToListAsync();

            return ticketsWithTechnicianName;
        }


        public async Task<IEnumerable<Ticket>> GetFilteredTicketsAsync(
            string status,
            string submitter,
            string subCategory,
            string technician,
            string urgencyLevel,
            string site)
        {
            var query = _context.Tickets.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(t => t.TicketStatus != null && t.TicketStatus.Name == status);
            }

            if (!string.IsNullOrWhiteSpace(submitter))
            {
                query = query.Where(t => t.User != null && t.User.UserName == submitter);
            }


            if (!string.IsNullOrWhiteSpace(technician))
            {
                
                query = query.Include(t => t.User) 
                             .Where(t => t.User != null && t.User.UserName == technician);
            }

            if (!string.IsNullOrWhiteSpace(urgencyLevel))
            {
                query = query.Where(t => t.UrgencyLevel != null && t.UrgencyLevel.Name == urgencyLevel);
            }


            if (!string.IsNullOrWhiteSpace(site))
            {
                query = query.Where(t => t.Site != null && t.Site.Name == site);
            }


            // Executing a query taking into account all filters
            var filteredTickets = await query
                .Include(t => t.TicketStatus) // Connecting the necessary connections
                .Include(t => t.UrgencyLevel)
                .Include(t => t.Site)
                .ToListAsync();

            return filteredTickets;
        }

    }

    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

}
