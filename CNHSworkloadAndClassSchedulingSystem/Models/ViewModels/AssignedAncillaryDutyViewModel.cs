namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class AssignedAncillaryDutyViewModel
    {
        public string DutyName { get; set; } = string.Empty;

        public int WeeklyMinutes { get; set; }

        public bool CountsTowardWorkload { get; set; }
    }
}