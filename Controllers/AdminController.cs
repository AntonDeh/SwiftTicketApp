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

        // Method for adding a new user
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Вернуться к списку пользователей или к странице успеха
                }
                else
                {
                    // Обработать ошибки
                }
            }
            return View(model);
        }
    }
}
