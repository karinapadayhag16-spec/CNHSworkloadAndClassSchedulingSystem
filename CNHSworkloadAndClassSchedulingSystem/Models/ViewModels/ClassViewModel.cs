using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using CNHSworkloadAndClassSchedulingSystem.Models.Enums;

namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class ClassViewModel
    {
        [Required]
        public int SchoolYearID { get; set; }

        [Required]
        public Department? Department { get; set; }

        [Required]
        [Range(7, 12, ErrorMessage = "Select a valid grade level.")]
        public int GradeLevel { get; set; }
        [Required]
        [StringLength(100)]
        public string SectionName { get; set; } = string.Empty;
        public int? AdviserID { get; set; }
        // Dropdown data
        public List<SelectListItem> SchoolYears { get; set; } = new();
        public List<SelectListItem> Departments { get; set; } = new();
        public List<SelectListItem> GradeLevels { get; set; } = new();
        public List<SelectListItem> Advisers { get; set; } = new();
        public int ClassSectionID { get; set; }
    }
}