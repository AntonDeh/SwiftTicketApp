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

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public IActionResult CreateRequest()
        {
            var model = new CreateTicketViewModel
            {
                // Populate lists if they need to be dynamic
                AvailableSites = GetAvailableSites(),
                AvailableCategories = GetAvailableCategories(),
                AvailableUrgencies = GetAvailableUrgencies()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(CreateTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Processing ticket creation
                var result = await _ticketService.CreateTicketAsync(model);
                if (result.Success)
                {
                    // Handling successful ticket creation
                    return RedirectToAction("SuccessPage"); // Redirect to success page
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

            // If something goes wrong, return the model back to the view
            model.AvailableSites = GetAvailableSites();
            model.AvailableCategories = GetAvailableCategories();
            model.AvailableUrgencies = GetAvailableUrgencies();
            return View(model);
        }

        // Stubs for methods for obtaining data for drop-down lists
        private List<SelectListItem> GetAvailableSites() => new List<SelectListItem>();
        private List<SelectListItem> GetAvailableCategories() => new List<SelectListItem>();
        private List<SelectListItem> GetAvailableUrgencies() => new List<SelectListItem>();


    }

}
