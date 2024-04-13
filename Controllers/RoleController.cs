using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwiftTicketApp.Models;
using SwiftTicketApp.ViewModels.Roles;


namespace SwiftTicketApp.Controllers
{
    [Authorize(Roles = "Admin")]    // Access to role management is only for administrators
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View(model: new RoleViewModel { RoleName = "" });
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(model.RoleName));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // Displays a form for editing an existing role
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel { Id = role.Id, RoleName = role.Name };
            return View(model);
        }
        // POST: /Role/Edit/
        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }
        // GET: /Role/Delete/
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            if (role.Name == null)
            {
                throw new InvalidOperationException("Role name cannot be null");
            }
            var model = new RoleViewModel { Id = role.Id, RoleName = role.Name };
            return View(model);
        }

        // POST: /Role/Delete/
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // If we get to this point, something went wrong, show form again
            return View("Delete", model: new RoleViewModel { Id = id, RoleName = role.Name = role.Name! });
        }

        //GET: /Role/ManageUserRoles
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            var users = await userManager.Users.ToListAsync();
            var model = new List<UserRoleViewModel>();
            var allRoles = roleManager.Roles.ToList();

            foreach (var user in users)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var currentRoleName = userRoles.FirstOrDefault() ?? "Not Assigned";

                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName ?? "Unknown",
                    CurrentRoleName = currentRoleName,
                    AvailableRoles = allRoles.Select(r => new SelectListItem
                    {
                        Value = r.Id,
                        Text = r.Name,
                        Selected = userRoles.Any(ur => ur == r.Name)
                    })
                };

                model.Add(userRoleViewModel);
            }

            return View(model);
        }
        // POST: /Role/ManageUserRoles
        [HttpPost]
        public async Task<IActionResult> UpdateUserRoles(List<UserRoleViewModel> model)
        {
            foreach (var userRoleViewModel in model)
            {
                var user = await userManager.FindByIdAsync(userRoleViewModel.UserId);

                if (user != null)
                {
                    var currentRoles = await userManager.GetRolesAsync(user);

                    // Check if a new role is selected
                    if (!string.IsNullOrEmpty(userRoleViewModel.CurrentRoleId))
                    {
                        // Remove all current roles except the new role
                        await userManager.RemoveFromRolesAsync(user, currentRoles);

                        // Add the new role if it exists
                        var newRole = await roleManager.FindByIdAsync(userRoleViewModel.CurrentRoleId);
                        if (newRole != null)
                        {
                            // Check for null before accessing the Name property
                            if (!string.IsNullOrEmpty(newRole.Name))
                            {
                                await userManager.AddToRoleAsync(user, newRole.Name);
                                userRoleViewModel.CurrentRoleName = newRole.Name;
                            }
                        }
                    }
                }
            }

            return RedirectToAction("ManageUserRoles");
        }
    }
}
