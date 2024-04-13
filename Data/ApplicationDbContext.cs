using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SwiftTicketApp.Models; 

namespace SwiftTicketApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> User) : IdentityDbContext(User)
    {

        private readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
    {
        builder
            .AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
            .AddConsole();
    });

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(loggerFactory);
        // Другие настройки вашего контекста данных
    }
        // DbSets represent collections of each entity that can be queried from the database
        public DbSet<Ticket> Tickets { get; set; } // Represents the Tickets table
        public DbSet<ServiceHistory> ServiceHistories { get; set; } // Represents the ServiceHistories table
        public DbSet<Comment> Comments { get; set; } // Represents the Comments table
        public DbSet<Site> Sites { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UrgencyLevel> UrgencyLevels { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketAssignment> TicketAssignments { get; set; }




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
                .HasForeignKey(c => c.UserId) // Defines the foreign key in the Comment entity pointing to User
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete to avoid cycles or multiple cascade paths

            modelBuilder.Entity<User>()
                .HasMany(u => u.ServiceHistories) // Specifies that a User has many ServiceHistories
                .WithOne(s => s.User) // Specifies that a ServiceHistory record is associated with one User
                .HasForeignKey(s => s.UserId) // Defines the foreign key in the ServiceHistory entity pointing to User
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete to avoid cycles or multiple cascade paths
            // Initial data load for Sites
            modelBuilder.Entity<Site>().HasData(
                new Site { Id = 1, Name = "IDC" },
                new Site { Id = 2, Name = "PTK" },
                new Site { Id = 3, Name = "JER" }
            );

            // Initial data loading for Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Tech Support" },
                new Category { Id = 2, Name = "Purchasing" },
                new Category { Id = 3, Name = "Environment" }
            );

            // Initial data loading for UrgencyLevels
            modelBuilder.Entity<UrgencyLevel>().HasData(
                new UrgencyLevel { Id = 1, Name = "Low" },
                new UrgencyLevel { Id = 2, Name = "High" },
                new UrgencyLevel { Id = 3, Name = "Normal" }
            );

            modelBuilder.Entity<TicketStatus>().HasData(
                new TicketStatus { Id = 1, Name = "New" },
                new TicketStatus { Id = 2, Name = "Assigned" },
                new TicketStatus { Id = 3, Name = "Closed" },
                new TicketStatus { Id = 4, Name = "User respond" },
                new TicketStatus { Id = 5, Name = "Closed By User" },
                new TicketStatus { Id = 6, Name = "Pending Supply" }
    );
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
            var hasher = new PasswordHasher<User>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var adminUser = new User
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
            modelBuilder.Entity<User>().HasData(adminUser);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(adminUserRole);
        }
    }
}