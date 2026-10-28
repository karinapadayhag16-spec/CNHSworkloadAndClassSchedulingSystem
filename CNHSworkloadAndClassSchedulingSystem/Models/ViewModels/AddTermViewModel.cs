using System.ComponentModel.DataAnnotations;

namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class AddTermViewModel
    {
        public string? SchoolYearLabel { get; set; }

        [Required]
        [Range(1, 100)]
        [Display(Name = "Term Number")]
        public int TermNumber { get; set; }

        [Required]
        [Range(1, 365)]
        [Display(Name = "Instructional Days")]
        public int InstructionalDays { get; set; }
    }
}
