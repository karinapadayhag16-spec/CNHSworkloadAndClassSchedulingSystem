namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class TeacherAvailability
    {
        public int TeacherAvailabilityID { get; set; }

        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public int TimePeriodID { get; set; }
        public TimePeriod TimePeriod { get; set; }

        public bool IsAvailable { get; set; }
    }
}
