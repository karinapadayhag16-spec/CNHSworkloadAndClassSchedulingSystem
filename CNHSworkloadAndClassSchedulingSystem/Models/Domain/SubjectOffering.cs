namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class SubjectOffering
    {
        public int SubjectOfferingID { get; set; }

        public int SubjectID { get; set; }
        public Subject Subject { get; set; }

        public int ClassSectionID { get; set; }
        public ClassSection ClassSection { get; set; }

        public int RequiredMinutesPerWeek { get; set; }

        public int TermID { get; set; }
        public Term Term { get; set; }
    }
}
