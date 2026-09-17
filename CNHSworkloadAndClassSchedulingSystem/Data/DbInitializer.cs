using Microsoft.AspNetCore.Identity;

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
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Models.ApplicationUser>>();

            // Create roles
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create default admin user if it doesn't exist
            var adminEmail = "admin@localhost";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new Models.ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "P@ssw0rd!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            // Note: MasterTeachers accounts are created by Admin users at runtime; no seed user is created here.
        }
    }
}
