using Microsoft.AspNetCore.Mvc;

namespace SwiftTicketApp.Controllers
{
    public class TicketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        // Action for the Create Service Request page
        public IActionResult CreateRequest()
        {
            return View();
        }

        // Action for the ticket search page
        public IActionResult FindTicket()
        {
            return View();
        }
    }
}
