using CNHSworkloadAndClassSchedulingSystem.Models.Enums;

namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class ClassSection
    {
        public int ClassSectionID { get; set; }

        public Department Department { get; set; }
        public int GradeLevel { get; set; }
        public string SectionName { get; set; }

        public int AdviserID { get; set; }
        public Teacher Adviser { get; set; }

        public int SchoolYearID { get; set; }
        public SchoolYear SchoolYear { get; set; }
    }
}
