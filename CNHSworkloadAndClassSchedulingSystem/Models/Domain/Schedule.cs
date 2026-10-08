namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int GradeLvl { get; set; }
        public string? SectionId { get; set; }
        public string? Dayset { get; set; }
        public string? Department { get; set; }
        public string? TeacherSet { get; set; }
        public string? SchoolTimeId { get; set; }
    }
}
