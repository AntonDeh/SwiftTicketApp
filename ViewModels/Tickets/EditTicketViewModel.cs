﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SwiftTicketApp.ViewModels.Tickets
{
    public class EditTicketViewModel
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;


        public int CurrentSite { get; set; }
        public string Category { get; set; } = string.Empty;
        public string SubCategory { get; set; } = string.Empty;
        public int Urgency { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string LabLocation { get; set; } = string.Empty;


        public int StatusId { get; set; }
        public List<SelectListItem> Statuses { get; set; } = new List<SelectListItem>();

   
    }
}
