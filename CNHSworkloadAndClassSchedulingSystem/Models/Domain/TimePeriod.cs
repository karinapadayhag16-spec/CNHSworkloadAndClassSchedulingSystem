using System;

namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class TimePeriod
    {
        public int TimePeriodId { get; set; }
        public int PeriodNumber { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBreak { get; set; }
        // Foreign key to TimeSchedule
        public int TimeScheduleId { get; set; }
        public TimeSchedule TimeSchedule { get; set; } = null!;
    }
}