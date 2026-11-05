
using System;
using System.ComponentModel.DataAnnotations;

namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class TimePeriodViewModel
    {
        [Required]
        [Range(1, 100)]
        public int PeriodNumber { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public bool IsBreak { get; set; }
    }
}
