using CNHSworkloadAndClassSchedulingSystem.Models.Enums;

namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class TimePeriod
    {
        public int TimePeriodId { get; set; }
        public int PeriodNumber { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Department Department { get; set; }
        public int TermID { get; set; }
        public Term Term { get; set; }
        public bool IsBreak { get; set; }
    }
}
