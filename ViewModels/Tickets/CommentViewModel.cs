using SwiftTicketApp.Models;

namespace SwiftTicketApp.ViewModels.Tickets
{
    public class CommentViewModel
    {
        public string Content { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
