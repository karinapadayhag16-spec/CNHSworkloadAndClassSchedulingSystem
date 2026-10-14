using System.ComponentModel.DataAnnotations;

namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class AddSchoolYearViewModel
    {
        [Required]
        [Range(2000, 2100)]
        [Display(Name = "Start Year")]
        public int StartYear { get; set; }

        [Required]
        [Range(2000, 2100)]
        [Display(Name = "End Year")]
        public int EndYear { get; set; }

        [Display(Name = "Current School Year")]
        public bool IsCurrent { get; set; }
    }
}
