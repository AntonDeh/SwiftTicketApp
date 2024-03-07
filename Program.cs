using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SwiftTicketApp.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure CORS to allow requests from your Angular client

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Add DbContext

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SwiftTicketConnectionString")));

// Adding Identity services to the application

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()   // Adds role support
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Additional configuration of services, for example, Entity Framework, Identity, etc.
// builder.Services.Add...

var app = builder.Build();

// Setting up an HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAngularDevOrigin"); // Apply CORS policy

app.UseAuthentication(); // This before app.UseAuthorization();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.UseEndpoints(endpoints => { _ = endpoints.MapControllers(); });

// Setup to serve SPA Angular on the server side through a proxy in development mode
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "Angular"; // Path to your Angular client application

    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200"); // URL of your Angular development server
    }
});

app.Run();
