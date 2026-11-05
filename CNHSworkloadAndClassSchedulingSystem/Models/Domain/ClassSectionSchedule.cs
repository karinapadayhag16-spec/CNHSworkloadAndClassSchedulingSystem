namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class ClassSectionSchedule
    {
        public int ClassSectionScheduleId { get; set; }
        // Foreign key to Class Section class
        public int ClassSectionID { get; set; }
        public ClassSection ClassSection { get; set; } = null!;
        // Assigned Schedule Version
        public int TimeScheduleId { get; set; }
        public TimeSchedule TimeSchedule { get; set; } = null!;
        // Term
        public int TermID { get; set; }
        public Term Term { get; set; } = null!;
    }
}