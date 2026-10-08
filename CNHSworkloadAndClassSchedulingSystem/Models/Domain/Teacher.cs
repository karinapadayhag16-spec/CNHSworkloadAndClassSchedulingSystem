using CNHSworkloadAndClassSchedulingSystem.Models.Enums;
using System.Reflection;

namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class Teacher
    {
        public int TeacherID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Department Department { get; set; }

        public int SchoolYearID { get; set; }
        public SchoolYear SchoolYear { get; set; }

        public ICollection<TeacherExpertise> SubjectExpertise { get; set; }
            = new List<TeacherExpertise>();

    }
}
