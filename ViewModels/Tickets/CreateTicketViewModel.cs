using Microsoft.AspNetCore.Mvc.Rendering;

namespace SwiftTicketApp.ViewModels.Tickets
{
    public class CreateTicketViewModel
    {
        private List<IFormFile> attachments = new List<IFormFile>();

        public int CurrentSite { get; set; }
        public string Category { get; set; } = string.Empty;
        public string SubCategory { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string LabLocation { get; set; } = string.Empty;
        public int Urgency { get; set; }
        public string CC { get; set; } = string.Empty;

        // To upload files
        public List<IFormFile> Attachments
        {
            get => attachments;
            set => attachments = value;
        }

        // Lists for dropdown menus if they are filled dynamically
        public List<SelectListItem> AvailableSites { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableCategories { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableUrgencies { get; set; } = new List<SelectListItem>();

    }

}
