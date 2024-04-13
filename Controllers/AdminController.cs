using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SwiftTicketApp.Models;
using SwiftTicketApp.ViewModels.Admin;


namespace SwiftTicketApp.Controllers
{
    

    [Authorize(Roles = "Admin")] // Access to user management is for administrators only
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
            var model = new AddUserViewModel();

            // Filling the list with roles
            model.Roles = _roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToList();

            return View(model); // Returning the prepared model
        }

        // POST: Admin/AddUser - Method for adding a new user
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            // Prepare a list of roles to return to the form in case of error
            PrepareRoles(model);

            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Add a user to the selected role
                    if (!string.IsNullOrWhiteSpace(model.SelectedRole))
                    {
                        var addToRoleResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                        if (!addToRoleResult.Succeeded)
                        {
                            // Handle errors when adding to a role
                            foreach (var error in addToRoleResult.Errors)
                            {
                                ModelState.AddModelError("", $"Error adding user to role: {error.Description}");
                            }

                            // Return to the add form with error information
                            return View(model);
                        }
                    }

                    // User successfully added and assigned to role, redirect to user list
                    return RedirectToAction("UsersList");
                }
                else
                {
                    // If there are errors when creating a user, add them to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            // If the model does not pass validation or if there are errors when creating the user,
            // return to model view to show errors
            return View(model);
        }

        private void PrepareRoles(AddUserViewModel model)
        {
            model.Roles = _roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToList();
        }

        // POST: Admin/DeleteUser
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    // User deleted successfully
                    return RedirectToAction("UsersList");
                }
                else
                {
                    // Return a response with error code 500 (Internal Server Error)
                    // and send an error message
                    return StatusCode(500, "User could not be deleted. Please try again.");
                }
            }

            // User not found or other error
            return RedirectToAction("UsersList");
        }
        // GET: Admin/EditUser
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("UsersList");
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? "Unknown",
                UserName = user.UserName ?? "Unknown"
            };

            return View(model);
        }

        // POST: Admin/EditUser
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction("UsersList");
                }

                user.Email = model.Email;
                user.UserName = model.UserName;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    // Error handling when updating a user
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(model);
                }

                // Optional: Password update if provided
                if (!string.IsNullOrWhiteSpace(model.NewPassword)) // Use NewPassword for clarity
                {
                    // Remove the old password if it exists
                    var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                    if (!removePasswordResult.Succeeded)
                    {
                        // If there was an issue removing the old password, return the errors
                        foreach (var error in removePasswordResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                    // Add the new password
                    var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                    if (!addPasswordResult.Succeeded)
                    {
                        // Handle errors for adding the new password
                        foreach (var error in addPasswordResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                // Redirect to the user list after successful update
                return RedirectToAction("UsersList");
            }
            // If we get here, something was wrong with the model
            return View(model);
        }
    }
}
