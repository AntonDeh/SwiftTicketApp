using System.ComponentModel.DataAnnotations;

namespace SwiftTicketApp.Models
{
    // Definition for service location (for example, office or location)
    public class Site
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
    }

    // Definition for Service Request Category
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
    }

    // Determining the Urgency Level of a Service Request
    public class UrgencyLevel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
    }
}
