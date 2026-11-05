
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class TimeScheduleViewModel
    {
        [Required(ErrorMessage = "Schedule name is required.")]
        [StringLength(100)]
        [Display(Name = "Schedule Name")]
        public string ScheduleName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(500)]
        [Display(Name = "Reason for Changes")]
        public string? ChangeReason { get; set; }

        public List<TimePeriodViewModel> TimePeriods { get; set; }
            = new List<TimePeriodViewModel>();
    }
}
