using CNHSworkloadAndClassSchedulingSystem.Models.Enums;

namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class SchoolYear
    {
        public int SchoolYearId { get; set; }
        public string? YearLabel { get; set; }
        public SchoolYearStatus Status { get; set; }
            = SchoolYearStatus.Setup;
        public ICollection<Term> Terms { get; set; }
            = new List<Term>();
    }
}

