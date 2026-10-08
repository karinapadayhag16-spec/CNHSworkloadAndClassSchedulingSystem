using Microsoft.AspNetCore.Identity;

namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public bool IsActive { get; set; }
    }
}
