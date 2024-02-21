using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SwiftTicketApp.Data
{
    // ApplicationDbContext class that inherits from IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext
    {
        // Constructor accepting DbContextOptions of ApplicationDbContext and passing it to the base class constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Additional DbSets for your entities can be added here

    }
}
