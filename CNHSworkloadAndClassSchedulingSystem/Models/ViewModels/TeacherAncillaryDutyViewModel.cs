
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class TeacherAncillaryDutyViewModel
    {
        public int TeacherID { get; set; }

        public int TeacherAssignmentID { get; set; }

        public string TeacherName { get; set; } = string.Empty;

        public string SchoolYearLabel { get; set; } = string.Empty;
        public int TermID { get; set; }

        public List<SelectListItem> AvailableTerms { get; set; } = new();

        [Required(ErrorMessage = "Please select an ancillary duty.")]
        public int? AncillaryDutyID { get; set; }

        [Range(0, 1800, ErrorMessage = "Weekly minutes must be between 0 and 1800.")]
        public int WeeklyMinutes { get; set; }

        public List<SelectListItem> AvailableDuties { get; set; }
            = new();
        public List<AssignedAncillaryDutyViewModel> AssignedDuties
        { get; set; } = new();
    }
}
