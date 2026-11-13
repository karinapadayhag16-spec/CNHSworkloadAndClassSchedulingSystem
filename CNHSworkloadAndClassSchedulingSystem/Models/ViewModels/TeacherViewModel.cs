using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using CNHSworkloadAndClassSchedulingSystem.Models.Enums;

namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class TeacherViewModel
    {
        public int TeacherID { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a department.")]
        public Department? Department { get; set; }

        public int SchoolYearID { get; set; }

        // Dropdown data
        public List<SelectListItem> Departments { get; set; } = new();
    }
}