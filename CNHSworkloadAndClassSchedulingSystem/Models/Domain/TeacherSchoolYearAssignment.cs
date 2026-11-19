
using CNHSworkloadAndClassSchedulingSystem.Models.Enums;

namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class TeacherSchoolYearAssignment
    {
        public int TeacherAssignmentID { get; set; }

        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; } = null!;

        public int SchoolYearID { get; set; }
        public SchoolYear SchoolYear { get; set; } = null!;

        public Department Department { get; set; }

        // Ancillary duties assigned during this school year.
        public ICollection<TeacherAncillaryDuty> AncillaryDuties
        { get; set; } = new List<TeacherAncillaryDuty>();
    }
}
