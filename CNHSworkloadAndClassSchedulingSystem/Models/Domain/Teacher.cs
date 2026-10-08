namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Department { get; set; }
        public string? Gender { get; set; }
        public string? SubExpertise { get; set; }
        public int ActualTeacherMins { get; set; }
    }
}
