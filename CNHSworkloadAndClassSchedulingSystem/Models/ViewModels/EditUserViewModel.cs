using System.ComponentModel.DataAnnotations;

namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Username")]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}
