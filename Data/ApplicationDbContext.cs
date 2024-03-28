using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SwiftTicketApp.Models; 

namespace SwiftTicketApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {

        // DbSets represent collections of each entity that can be queried from the database
        public DbSet<Ticket> Tickets { get; set; } // Represents the Tickets table
        public DbSet<ServiceHistory> ServiceHistories { get; set; } // Represents the ServiceHistories table
        public DbSet<Comment> Comments { get; set; } // Represents the Comments table

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Model configuration and relationship definitions, if needed
            // For example, defining foreign keys, indexes, many-to-many relationships, etc.

            // Configuration for User entity
            modelBuilder.Entity<User>()
                .HasMany(u => u.TicketsCreated) // Specifies that a User has many TicketsCreated
                .WithOne(t => t.User) // Specifies that a Ticket is associated with one User
                .HasForeignKey(t => t.UserId) // Defines the foreign key in the Ticket entity pointing to User
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete to avoid cycles or multiple cascade paths

            // Add here the new configuration for the Comment and Ticket relationship
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Ticket) // Indicates that a Comment is associated with a single Ticket
                .WithMany(t => t.Comments) // Indicates that a Ticket can have many Comments
                .HasForeignKey(c => c.TicketId) // Specifies the foreign key in the Comment entity
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete to maintain data integrity


            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments) // Specifies that a User has many Comments
                .WithOne(c => c.User) // Specifies that a Comment is associated with one User
                .HasForeignKey(c => c.UserId); // Defines the foreign key in the Comment entity pointing to User


            modelBuilder.Entity<User>()
                .HasMany(u => u.ServiceHistories) // Specifies that a User has many ServiceHistories
                .WithOne(s => s.User) // Specifies that a ServiceHistory record is associated with one User
                .HasForeignKey(s => s.UserId); // Defines the foreign key in the ServiceHistory entity pointing to User


            // Initialize an admin role and user at startup
            InitializeAdminUser(modelBuilder);

        }
        // Method to initialize an admin user and role
        private static void InitializeAdminUser(ModelBuilder modelBuilder)
        {
            // Generating GUID for role and user
            var adminRoleId = Guid.NewGuid().ToString();
            var adminUserId = Guid.NewGuid().ToString();

            // Admin role
            var adminRole = new IdentityRole
            {
                Id = adminRoleId, // Use a GUID or predefined ID
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            // Admin user
            var hasher = new PasswordHasher<IdentityUser>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var adminUser = new IdentityUser
            {
                Id = adminUserId, // Use a GUID or predefined ID
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "1@Password"), // Use a secure password
                SecurityStamp = string.Empty
            };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Assign admin user to admin role
            var adminUserRole = new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            };

            // Seed admin role and user
            modelBuilder.Entity<IdentityRole>().HasData(adminRole);
            modelBuilder.Entity<IdentityUser>().HasData(adminUser);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(adminUserRole);
        }
    }
}