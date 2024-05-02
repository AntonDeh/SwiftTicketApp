using System.ComponentModel.DataAnnotations;

namespace SwiftTicketApp.ViewModels.Admin
{
    public class ReportViewModel
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int ClosedTicketsCount { get; set; } = 0;
    }
}
