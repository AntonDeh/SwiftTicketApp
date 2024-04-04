using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SwiftTicketApp.ViewModels.Tickets
{
    public class EditTicketViewModel
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;


        public string CurrentSite { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string SubCategory { get; set; } = string.Empty;
        public string Urgency { get; set; } = string.Empty;


        public int StatusId { get; set; }
        public List<SelectListItem> Statuses { get; set; } = new List<SelectListItem>();

   
    }
}
