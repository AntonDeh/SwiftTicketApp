using Microsoft.AspNetCore.Mvc.Rendering;
using SwiftTicketApp.Models;

namespace SwiftTicketApp.ViewModels.Tickets
{
    public class TechnicianDashboardViewModel
    {
        public IEnumerable<Ticket> Tickets { get; set; } = Enumerable.Empty<Ticket>();
        public List<SelectListItem> Statuses { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Submitters { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> SubCategories { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Technicians { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> UrgencyLevels { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Sites { get; set; } = new List<SelectListItem>();

        // Selected filter properties
        public string SelectedStatus { get; set; } = string.Empty;
        public string SelectedSubmitter { get; set; } = string.Empty;
        public string SelectedSubCategory { get; set; } = string.Empty;
        public string SelectedTechnician { get; set; } = string.Empty;
        public string SelectedUrgencyLevel { get; set; } = string.Empty;
        public string SelectedSite { get; set; } = string.Empty;
    }
}
