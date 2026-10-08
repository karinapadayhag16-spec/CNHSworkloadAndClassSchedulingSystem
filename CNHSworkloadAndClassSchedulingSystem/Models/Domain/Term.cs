namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class Term
    {
        public int TermID { get; set; }

        public int TermNumber { get; set; }

        public int SchoolYearID { get; set; }

        public SchoolYear SchoolYear { get; set; }
        public ICollection<TimePeriod> TimePeriods { get; set; } = new List<TimePeriod>();
    }
}
