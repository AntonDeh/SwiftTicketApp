using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwiftTicketApp.ViewModels.Roles;


namespace SwiftTicketApp.Controllers
{
    [Authorize(Roles = "Admin")]    // Access to role management is only for administrators
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
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
                Console.WriteLine($"User: {user.UserName}, Roles: {string.Join(", ", userRoles)}");
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
                var currentRoles = await userManager.GetRolesAsync(user);

                // Check if a new role is selected
                if (!string.IsNullOrEmpty(userRoleViewModel.CurrentRoleId))
                {
                    // Remove all current roles except the new role
                    await userManager.RemoveFromRolesAsync(user, currentRoles.Except(new[] { userRoleViewModel.CurrentRoleName }));

                    // Add the new role
                    var newRole = await roleManager.FindByIdAsync(userRoleViewModel.CurrentRoleId);
                    if (newRole != null)
                    {
                        await userManager.AddToRoleAsync(user, newRole.Name);
                        userRoleViewModel.CurrentRoleName = newRole.Name;
                    }
                }
            }

            return RedirectToAction("ManageUserRoles");
        }

        /*        // POST: /Role/ManageUserRoles
                [HttpPost]
                public async Task<IActionResult> UpdateUserRoles(List<UserRoleViewModel> model, string roleId)
                {
                    var role = await roleManager.FindByIdAsync(roleId);
                    if (role == null)
                    {
                        ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                        return View("NotFound");
                    }

                    for (int i = 0; i < model.Count; i++)
                    {
                        var user = await userManager.FindByIdAsync(model[i].UserId);


                        IdentityResult result ;
                        if (model[i].IsSelected && !await userManager.IsInRoleAsync(user, role.Name))
                        {
                            result = await userManager.AddToRoleAsync(user, role.Name);
                        }
                        else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                        {
                            result = await userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                        else
                        {
                            continue; // Do not change anything if the role membership status has not changed
                        }

                        if (result != null && !result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            // Return to current view to show errors
                            return View("ManageUserRoles", model);
                        }
                    }

                    // After updating user roles, redirect to the roles page
                    return RedirectToAction("Edit", new { Id = roleId });
                }
        */
    }
}
