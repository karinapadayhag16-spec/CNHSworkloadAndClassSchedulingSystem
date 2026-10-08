namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class ClassSection
    {
        public int ClassSectionId { get; set; }
        public string? Department { get; set; }
        public int GradeLvl { get; set; }
        public string? SectionName { get; set; }
        public string? Adviser { get; set; }
    }
}
