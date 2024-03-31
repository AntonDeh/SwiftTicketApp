﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SwiftTicketApp.ViewModels.Admin;


namespace SwiftTicketApp.Controllers
{
    

    [Authorize(Roles = "Admin")] // Access to user management is for administrators only
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
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
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);

                
                if (result.Succeeded)
                {
                    // Add a user to the selected role
                    if (!string.IsNullOrWhiteSpace(model.SelectedRole))
                    {
                        var addToRoleResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                        if (!addToRoleResult.Succeeded)
                        {
                            // Error handling when adding to a role
                        }
                    }

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

    }
}
