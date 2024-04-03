using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SwiftTicketApp.Interfaces;
using SwiftTicketApp.ViewModels.Tickets;

namespace SwiftTicketApp.Controllers
{
    public class TicketController : Controller
    {
        // ticket processing service
        private readonly ITicketService _ticketService;
        private readonly UserManager<IdentityUser> _userManager;

        public TicketController(ITicketService ticketService, UserManager<IdentityUser> userManager)
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
                return RedirectToAction("Login", "Account"); // If the user is not authenticated, we redirect to the login page
            }

            var tickets = await _ticketService.GetTicketsByUserIdAsync(userId); // Retrieving user tickets

            return View(tickets); // Passing a list of tickets to the view
        }

        // Stubs for methods for obtaining data for drop-down lists
        private List<SelectListItem> GetAvailableSites() => new List<SelectListItem>();
        private List<SelectListItem> GetAvailableCategories() => new List<SelectListItem>();
        private List<SelectListItem> GetAvailableUrgencies() => new List<SelectListItem>();


    }

}
