namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class Schedule
    {
        public int ScheduleID { get; set; }
        public int SubjectOfferingID { get; set; }
        public SubjectOffering SubjectOffering { get; set; }
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }

        public int TimePeriodID { get; set; }
        public TimePeriod TimePeriod { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
