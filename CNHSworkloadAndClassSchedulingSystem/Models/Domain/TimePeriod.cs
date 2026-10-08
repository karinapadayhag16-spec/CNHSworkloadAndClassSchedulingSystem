namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class TimePeriod
    {
        public int SchoolTimeId { get; set; } 
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public int Period { get; set; }
        public string Department { get; set; }
        public int SchoolYear { get; set; }
    }
}
