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
        private readonly UserManager<User> _userManager;

        public TicketService(ApplicationDbContext context, UserManager<User> userManager)
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
                    TechnicianId = null

                };

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
            var ticketsWithTechnicianName = await _context.Tickets
                .Include(t => t.Technician)
                .Select(t => new SelectListItem
                {
                    Value = t.TicketId.ToString(),
                    Text = t.Technician != null ? $"{t.Title} - {t.Technician.UserName}" : $"{t.Title} - Not_assigned"
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
                var urgencyLevelId = int.Parse(urgencyLevel);
                query = query.Where(t => t.Urgency == urgencyLevelId);
            }

            if (!string.IsNullOrWhiteSpace(site))
            {
                var siteId = int.Parse(site); 
                query = query.Where(t => t.CurrentSite == siteId);
            }

            // Executing a query taking into account all filters
            var filteredTickets = await query
                .Include(t => t.User)
                .Include(t => t.TicketStatus) // Connecting the necessary connections
                .Include(t => t.UrgencyLevel)
                .Include(t => t.Site)
                
                .ToListAsync();

            return filteredTickets;
        }
        public async Task<ServiceResponse> UpdateTicketStatusAsync(int ticketId, string userId, string newStatus)
        {
            var serviceResponse = new ServiceResponse();

            // Checking user role
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Technician")))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Access denied.";
                return serviceResponse;
            }

            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Ticket not found.";
                return serviceResponse;
            }

            var status = await _context.TicketStatuses.FirstOrDefaultAsync(s => s.Name.ToLower() == newStatus.ToLower());
            if (status == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Status not found.";
                return serviceResponse;
            }

            ticket.StatusId = status.Id;
            try
            {
                _context.Update(ticket);
                await _context.SaveChangesAsync();
                serviceResponse.Success = true;
                serviceResponse.Message = "Ticket status updated successfully.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error updating ticket status: {ex.Message}";
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse> AssignTicketToTechnicianAsync(int ticketId, string userId)
        {
            var serviceResponse = new ServiceResponse();

            var ticket = await _context.Tickets.SingleOrDefaultAsync(t => t.TicketId == ticketId);
            var technician = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            if (ticket == null || technician == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ticket == null ? "Ticket not found." : "Technician not found.";
                return serviceResponse;
            }

            // Ensure that the status is "Assigned"
            var assignedStatus = await _context.TicketStatuses.FirstOrDefaultAsync(s => s.Name == "Assigned");
            if (assignedStatus == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Assigned status not found.";
                return serviceResponse;
            }
            ticket.StatusId = assignedStatus.Id;

            // Update the TechnicianId directly
            ticket.TechnicianId = userId;

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.Success = true;
                serviceResponse.Message = "Ticket has been assigned to the technician.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error while assigning the ticket: {ex.Message}";
            }

            return serviceResponse;
        }
    }
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

}
