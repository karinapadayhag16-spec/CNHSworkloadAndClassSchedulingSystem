namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class SchoolYear
    {
        public int SchoolYearId { get; set; }
        public string? SchoolYearDuration { get; set; }
        public bool IsCurrent { get; set; }
    }
}
