using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SwiftTicketApp.ViewModels.Admin;


namespace SwiftTicketApp.Controllers
{
    [Authorize(Roles = "Admin")] // Access to user management is for administrators only

    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        // Method to display a list of users
        public IActionResult UsersList()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // GET: Admin/AddUser
        [HttpGet]
        public IActionResult AddUser()
        {
            return View(new AddUserViewModel()); // Returning a view with an empty model
        }

        // POST: Admin/AddUser - Method for adding a new user
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // If the user is successfully added, we redirect to the list of users
                    return RedirectToAction("UsersList");
                }
                else
                {
                    // If there are errors, add them to ModelState to display them on the form
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
