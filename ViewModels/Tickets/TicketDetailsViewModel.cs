using Microsoft.AspNetCore.Mvc.Rendering;

namespace SwiftTicketApp.ViewModels.Tickets
{
    public class TicketDetailsViewModel
    {
        public int TicketId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string SelectedTechnicianId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CurrentStatus { get; set; } = string.Empty;
        public List<SelectListItem> AvailableStatuses { get; set; } = new List<SelectListItem>();
        public string AssignedTechnician { get; set; } = string.Empty;
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public List<SelectListItem> AvailableTechnicians { get; set; } = new List<SelectListItem>();


    }
}

