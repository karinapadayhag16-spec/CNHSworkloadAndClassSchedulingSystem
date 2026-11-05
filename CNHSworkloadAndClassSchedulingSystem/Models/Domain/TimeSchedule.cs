using System;
using System.Collections.Generic;

namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class TimeSchedule
    {
        public int TimeScheduleId { get; set; }
        public string ScheduleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        // Version information
        public int VersionNumber { get; set; } = 1;
        // References the previous version
        public int? BasedOnTimeScheduleId { get; set; }
        public TimeSchedule? BasedOnTimeSchedule { get; set; }
        // Archive status
        public bool IsArchived { get; set; } = false;
        // Reason for creating this version
        public string? ChangeReason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Navigation properties
        public ICollection<TimePeriod> TimePeriods { get; set; }
            = new List<TimePeriod>();
        public ICollection<ClassSectionSchedule> ClassSectionSchedules
        { get; set; } = new List<ClassSectionSchedule>();
    }
}