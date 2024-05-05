using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SwiftTicketApp.Data;
using SwiftTicketApp.Services;
using SwiftTicketApp.Interfaces;
using SwiftTicketApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SwiftTicketConnectionString"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5, 
            maxRetryDelay: TimeSpan.FromSeconds(30), 
            errorNumbersToAdd: null); 
    }
    ));

// Adding Identity services to the application
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()   // Adds role support
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Register application services: controllers, data context, authentication services and custom services
builder.Services.AddControllersWithViews();

// Register HttpClient and MailgunEmailService
builder.Services.AddHttpClient();
builder.Services.AddSingleton<MailgunEmailService>();
// Ticket Service
builder.Services.AddScoped<ITicketService, TicketService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Make sure to call this before app.UseAuthorization();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Apply pending migrations and create database if not exists
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
app.Run();
