using CNHSworkloadAndClassSchedulingSystem.Models.Enums;

namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class TeacherIndexViewModel
    {
        public int TeacherID { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public Department Department { get; set; }

        public string Expertise { get; set; } = string.Empty;

        public int ActualTeachingMinutes { get; set; }

        public decimal ActualTeachingHours =>
            ActualTeachingMinutes / 60m;
        public int AncillaryDutyMinutes { get; set; }

        public int TotalWorkloadMinutes =>
            ActualTeachingMinutes + AncillaryDutyMinutes;

        public decimal TotalWorkloadHours =>
            TotalWorkloadMinutes / 60m;

        public int RemainingWorkloadMinutes =>
            Math.Max(0, 1800 - TotalWorkloadMinutes);

        public bool ExceedsMaximumWorkload =>
            TotalWorkloadMinutes > 1800;
        public List<AssignedAncillaryDutyViewModel> AssignedAncillaryDuties
        { get; set; } = new();
    }
}