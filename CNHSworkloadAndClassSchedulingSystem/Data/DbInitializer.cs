using System;
using System.Linq;
using CNHSworkloadAndClassSchedulingSystem.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CNHSworkloadAndClassSchedulingSystem.Data
{
    public static class DbInitializer
    {
        // Roles to ensure exist in the system
        public static readonly string[] Roles = new[] { "Admin", "MasterTeachers" };

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("DbInitializer");

            // Create roles
            foreach (var role in Roles)
            {
                try
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                        if (!roleResult.Succeeded)
                        {
                            var errors = string.Join(';', roleResult.Errors.Select(e => e.Description));
                            logger.LogError("Failed to create role '{Role}': {Errors}", role, errors);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception while creating role '{Role}'", role);
                }
            }

            // Create default admin user if it doesn't exist
            var adminEmail = "admin@localhost";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new Models.Domain.ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsActive = true,
                    FirstName = "Admin",
                    LastName = "User"
                };

                IdentityResult result;
                try
                {
                    result = await userManager.CreateAsync(adminUser, "P@ssw0rd!");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception while creating default admin user '{Email}'", adminEmail);
                    return;
                }

                if (result.Succeeded)
                {
                    var addRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (!addRoleResult.Succeeded)
                    {
                        var errors = string.Join(';', addRoleResult.Errors.Select(e => e.Description));
                        logger.LogError("Failed to add admin user to role 'Admin': {Errors}", errors);
                    }
                }
                else
                {
                    var errors = string.Join(';', result.Errors.Select(e => e.Description));
                    logger.LogError("Failed to create default admin user '{Email}': {Errors}", adminEmail, errors);
                }
            }
            else if (!adminUser.IsActive)
            {
                // If an admin user exists but is inactive, activate it
                adminUser.IsActive = true;
                var updateResult = await userManager.UpdateAsync(adminUser);
                if (!updateResult.Succeeded)
                {
                    var errors = string.Join(';', updateResult.Errors.Select(e => e.Description));
                    logger.LogError("Failed to activate existing admin user '{Email}': {Errors}", adminEmail, errors);
                }
            }
            // Note: MasterTeachers accounts are created by Admin users at runtime; no seed user is created here.
        }
    }
}
