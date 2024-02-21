using Microsoft.EntityFrameworkCore;
using SwiftTicketApp.Models; // Ensure you import the namespace where your models are defined

public class ApplicationDbContext : DbContext
{
    // Constructor that takes options of type DbContextOptions<ApplicationDbContext> and passes it to the base constructor of DbContext
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets represent collections of each entity that can be queried from the database
    public DbSet<User> Users { get; set; } // Represents the Users table
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

        // Similar configuration can be added for other models if required
    }
}
