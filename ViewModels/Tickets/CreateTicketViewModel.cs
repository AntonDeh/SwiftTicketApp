using Microsoft.AspNetCore.Mvc.Rendering;

namespace SwiftTicketApp.ViewModels.Tickets
{
    public class CreateTicketViewModel
    {
        public string CurrentSite { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; } // Если есть подкатегории
        public string Description { get; set; }
        public string MobileNumber { get; set; }
        public string LabLocation { get; set; }
        public string Urgency { get; set; }
        public string CC { get; set; }
        public List<IFormFile> Attachments { get; set; } // Для загрузки файлов

        // Списки для выпадающих меню, если они заполняются динамически
        public List<SelectListItem> AvailableSites { get; set; }
        public List<SelectListItem> AvailableCategories { get; set; }
        public List<SelectListItem> AvailableUrgencies { get; set; }

        // Можно добавить дополнительные поля по мере необходимости
    }

}
