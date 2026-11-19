
namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class AncillaryDuty
    {
        public int AncillaryDutyID { get; set; }
        public string DutyName { get; set; } = string.Empty;
        public bool CountsTowardWorkload { get; set; }
        public int? FixedWeeklyMinutes { get; set; }
        public ICollection<TeacherAncillaryDuty> TeacherAncillaryDuties
        { get; set; } = new List<TeacherAncillaryDuty>();
    }
}
