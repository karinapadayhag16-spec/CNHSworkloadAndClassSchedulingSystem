using System.ComponentModel.DataAnnotations;

namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class TeacherAncillaryDuty
    {
        public int TeacherAncillaryDutyID { get; set; }

        public int TeacherAssignmentID { get; set; }
        public TeacherSchoolYearAssignment TeacherAssignment { get; set; } = null!;

        public int TermID { get; set; }
        public Term Term { get; set; } = null!;

        public int AncillaryDutyID { get; set; }
        public AncillaryDuty AncillaryDuty { get; set; } = null!;

        public int WeeklyMinutes { get; set; }

        public bool CountsTowardWorkload { get; set; }
    }
}
