namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class SchoolYear
    {
        public int SchoolYearId { get; set; }
        public string? YearLabel { get; set; }
        public bool IsCurrent { get; set; }
        public ICollection<Term> Terms { get; set; } = new List<Term>();
    }
}

