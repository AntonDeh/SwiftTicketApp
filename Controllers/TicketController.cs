using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwiftTicketApp.Interfaces;
using SwiftTicketApp.Models;
using SwiftTicketApp.ViewModels.Tickets;

namespace SwiftTicketApp.Controllers
{
    public class TicketController : Controller
    {
        // ticket processing service
        private readonly ITicketService _ticketService;
        private readonly UserManager<User> _userManager;

        public TicketController(ITicketService ticketService, UserManager<User> userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
        }
        // GET : /Ticket/CreateRequestAsync
        [HttpGet]
        public async Task<IActionResult> CreateRequestAsync()
        {
            var model = new CreateTicketViewModel
            {
                // Populate lists if they need to be dynamic
                AvailableSites = await _ticketService.GetAvailableSitesAsync(),
                AvailableCategories = await _ticketService.GetAvailableCategoriesAsync(),
                AvailableUrgencies = await _ticketService.GetAvailableUrgenciesAsync()
            };

            return View(model);
        }
        // POST : /Ticket/CreateRequestAsync
        [HttpPost]
        public async Task<IActionResult> CreateRequest(CreateTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Getting the current user ID
                var userId = _userManager.GetUserId(User);

                // Checking if userId is not null
                if (userId == null)
                {
                    // If userId is null, the user is not authenticated
                    // You can redirect the user to the login page or add an error to ModelState
                    ModelState.AddModelError(string.Empty, "You must be logged in to submit a ticket.");
                    // Return the model to the view to display the error
                    return View(model);
                }

                // If the user is authenticated, process the ticket creation
                var result = await _ticketService.CreateTicketAsync(model, userId);
                if (result.Success)
                {
                    // Handling successful ticket creation
                    return RedirectToAction("MyTickets");
                }
                else
                {
                    // Adding errors to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            // If something goes wrong, return the model back to the view to display errors
            return View(model);
        }

        // GET: /Ticket/MyTickets
        [HttpGet]
        public async Task<IActionResult> MyTickets()
        {
            var userId = _userManager.GetUserId(User); // Get the current user ID

            if (userId == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to access this page.";
                return RedirectToAction("Login", "Account"); // If the user is not authenticated, we redirect to the login page
            }

            var tickets = await _ticketService.GetTicketsByUserIdAsync(userId); // Retrieving user tickets

            return View(tickets); // Passing a list of tickets to the view
        }
        // GET: /Ticket/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var model = new EditTicketViewModel
            {
                TicketId = ticket.TicketId,
                Description = ticket.Description,
                CurrentSite = ticket.CurrentSite,
                Category = ticket.Category,
                MobileNumber = ticket.MobileNumber,
                LabLocation = ticket.LabLocation,
                Urgency = ticket.Urgency
            };
            ViewBag.CurrentSite = new SelectList(await _ticketService.GetAvailableSitesAsync(), "Value", "Text", ticket.CurrentSite);
            ViewBag.Category = new SelectList(await _ticketService.GetAvailableCategoriesAsync(), "Value", "Text", ticket.Category);
            ViewBag.Urgency = new SelectList(await _ticketService.GetAvailableUrgenciesAsync(), "Value", "Text", ticket.Urgency);


            return View(model);
        }
        // POST: /Ticket/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(EditTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updateSuccess = await _ticketService.UpdateTicketAsync(model);
                if (updateSuccess)
                {
                    return RedirectToAction("MyTickets");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while updating the ticket.");
                }
            }

            return View(model);
        }
        // POST: /Ticket/CloseTicket
        [HttpPost]
        public async Task<IActionResult> CloseTicket(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to access this page.";
                return RedirectToAction("Login", "Account");
            }

            var response = await _ticketService.CloseTicketAsync(id, userId);
            if (response.Success)
            {
                return RedirectToAction("MyTickets");
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("MyTickets");
            }
        }
        // GET: /Ticket/ShowSearchForm
        [HttpGet]
        public IActionResult ShowSearchForm()
        {
            var viewModel = new TicketSearchViewModel();
            return View("TicketSearch", viewModel);
        }
        // GET: /Ticket/Search Ticket
        [HttpGet]
        public async Task<IActionResult> PerformSearch(string searchTerm)
        {
            var searchResults = await _ticketService.SearchTicketsAsync(searchTerm);
            var viewModel = new TicketSearchViewModel
            {
                SearchTerm = searchTerm,
                SearchResults = searchResults
            };
            return View("TicketSearch", viewModel);
        }
        // GET: /Ticket/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            var currentStatus = ticket.TicketStatus?.Name ?? "No Status";
            var availableStatuses = await _ticketService.GetAvailableStatusesAsync(); 

            var viewModel = new TicketDetailsViewModel
            {
                TicketId = ticket.TicketId,
                Description = ticket.Description,
                CurrentStatus = ticket.TicketStatus?.Name ?? "No Status",
                Status = ticket.TicketStatus?.Name ?? "No Status",
                CreatedAt = ticket.CreatedAt,
                AvailableStatuses = availableStatuses,

            };

            return View(viewModel);
        }
        // POST: /Ticket/AssignToMe
        [HttpPost]
        [Authorize(Roles = "Technician")]
        public async Task<IActionResult> AssignToMe(int ticketId)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User must be logged in to assign tickets.");
            }

            var statusResult = await _ticketService.UpdateTicketStatusAsync(ticketId, userId, "Assigned");
            if (!statusResult.Success)
            {
                TempData["ErrorMessage"] = statusResult.Message;
                return RedirectToAction("Details", new { id = ticketId });
            }

            var assignResult = await _ticketService.AssignTicketToTechnicianAsync(ticketId, userId);
            if (assignResult.Success)
            {
                TempData["Message"] = "Ticket assigned to you successfully!";
                return RedirectToAction("TechnicianDashboard");

            }
            else
            {
                TempData["ErrorMessage"] = assignResult.Message;
            }

            return RedirectToAction("TechnicianDashboard", new { id = ticketId });
        }


        // POST: /Ticket/UpdateStatus
        [HttpPost]
        [Authorize(Roles = "Admin,Technician")]
        public async Task<IActionResult> UpdateStatus(int ticketId, string newStatus)
        {
            var userId = _userManager.GetUserId(User); // Get the current user ID
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User must be logged in to assign tickets.");
            }

            var result = await _ticketService.UpdateTicketStatusAsync(ticketId, userId, newStatus); // Update ticket status

            if (result.Success)
            {
                TempData["Message"] = "Ticket status updated successfully!";
                return RedirectToAction("TechnicianDashboard");
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction("Details", new { id = ticketId });
        }

        // GET: /Ticket/ClosedTickets
        [HttpGet]
        public async Task<IActionResult> ClosedTickets()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to access this page.";
                return RedirectToAction("Login", "Account");
            }

            var closedTickets = await _ticketService.GetClosedTicketsByUserIdAsync(userId);
            var viewModel = new ClosedTicketsViewModel
            {
                ClosedTickets = closedTickets
            };

            return View(viewModel);
        }
        // GET: /Ticket/Dashboard
        [HttpGet]
        public async Task<IActionResult> TechnicianDashboard(
            string status, 
            string submitter, 
            string subCategory, 
            string technician, 
            string urgencyLevel,
            string site)
        {
            // We receive filtered tickets
            var filteredTickets = await _ticketService.GetFilteredTicketsAsync(
                status, 
                submitter, 
                subCategory, 
                technician, 
                urgencyLevel, 
                site);

            // Creating a ViewModel for a dashboard with filtered tickets and lists for dropdowns
            var viewModel = new TechnicianDashboardViewModel
            {
                Tickets = filteredTickets,
                Statuses = await _ticketService.GetAvailableStatusesAsync(),
                Submitters = await _ticketService.GetAvailableSubmittersAsync(),
                Technicians = await _ticketService.GetAvailableTechniciansAsync(),
                UrgencyLevels = await _ticketService.GetAvailableUrgenciesAsync(),
                Sites = await _ticketService.GetAvailableSitesAsync()
            };

            return View(viewModel);
        }
        // POST: /Ticket/Dashboard
        [HttpPost]
        public async Task<IActionResult> TechnicianDashboard(TechnicianDashboardViewModel model)
        {
            // Receive filtered tickets based on selected parameters
            model.Tickets = await _ticketService.GetFilteredTicketsAsync(
                model.SelectedStatus,
                model.SelectedSubmitter,
                model.SelectedSubCategory,
                model.SelectedTechnician,
                model.SelectedUrgencyLevel,
                model.SelectedSite);

            // Reloading lists for dropdowns
            model.Statuses = await _ticketService.GetAvailableStatusesAsync();
            model.Submitters = await _ticketService.GetAvailableSubmittersAsync();
            model.Technicians = await _ticketService.GetAvailableTechniciansAsync();
            model.UrgencyLevels = await _ticketService.GetAvailableUrgenciesAsync();
            model.Sites = await _ticketService.GetAvailableSitesAsync();

            // Returning the updated model to the view
            return View("TechnicianDashboard", model);
        }
        




        // Stubs for methods for obtaining data for drop-down lists
        private List<SelectListItem> GetAvailableSites() => new List<SelectListItem>();
        private List<SelectListItem> GetAvailableCategories() => new List<SelectListItem>();
        private List<SelectListItem> GetAvailableUrgencies() => new List<SelectListItem>();
        private List<SelectListItem> GetAvailableStatusesAsync() => new List<SelectListItem>();

    }

}
