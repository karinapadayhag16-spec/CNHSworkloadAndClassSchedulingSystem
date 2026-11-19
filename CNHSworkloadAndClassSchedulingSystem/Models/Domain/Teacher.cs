using CNHSworkloadAndClassSchedulingSystem.Models.Enums;
namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
        public class Teacher
        {
            public int TeacherID { get; set; }
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public bool IsArchived { get; set; } = false;
            // Subject expertise remains associated with the permanent teacher record.
            public ICollection<TeacherExpertise> SubjectExpertise { get; set; }
                = new List<TeacherExpertise>();

            // School-year-specific teacher assignments.
            public ICollection<TeacherSchoolYearAssignment> SchoolYearAssignments
            { get; set; } = new List<TeacherSchoolYearAssignment>();
        }
    }


