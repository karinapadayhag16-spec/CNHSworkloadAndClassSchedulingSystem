namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class SubjectViewModel
    {
        public string SubjectName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string GradeLevel { get; set; } = string.Empty;

        public int NumberOfClasses { get; set; }
    }
}