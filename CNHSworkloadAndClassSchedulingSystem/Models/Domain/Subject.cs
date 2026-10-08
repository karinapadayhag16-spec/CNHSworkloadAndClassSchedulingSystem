using CNHSworkloadAndClassSchedulingSystem.Models.Enums;

namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class Subject
    {
        public int SubjectID { get; set; }

        public string SubjectName { get; set; }

        public DifficultyLevel DifficultyLevel { get; set; }

        public ICollection<TeacherExpertise> TeacherExpertises { get; set; }
            = new List<TeacherExpertise>();

        public ICollection<SubjectOffering> SubjectOfferings { get; set; }
            = new List<SubjectOffering>();
    }
}

