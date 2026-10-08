namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string? Department { get; set; }

        public int GradeLvl { get; set; }
        public string? SectionId { get; set; }
        public string? Difficulty { get; set; }
        public int SubjectMins { get; set; }
    }
}
