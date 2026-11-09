using CNHSworkloadAndClassSchedulingSystem.Models.Domain;
using CNHSworkloadAndClassSchedulingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserSettingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSettingController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .OrderBy(user => user.LastName)
                .ThenBy(user => user.FirstName)
                .ToListAsync();

            return View(users);
        }

        public IActionResult AddUsers()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddUsers", model);
            }

            // Generate username using First Name and Last Name
            var baseUsername = $"{model.FirstName.Trim()}.{model.LastName.Trim()}"
                .ToLowerInvariant();

            var username = baseUsername;
            var suffix = 1;

            // Check if username already exists
            while (await _userManager.FindByNameAsync(username) != null)
            {
                username = $"{baseUsername}{suffix++}";
            }

            // Create new user
            var user = new ApplicationUser
            {
                UserName = username,
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                IsActive = true
            };

            // Save user with securely hashed password
            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View("AddUsers", model);
            }

            // Assign role
            var roleResult = await _userManager.AddToRoleAsync(user, "MasterTeachers");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View("AddUsers", model);
            }

            // Success message
            TempData["SuccessMessage"] =
                $"User '{username}' was created successfully.";

            // Return to User Setting Index
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ViewUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);

        }







    }
}
    
