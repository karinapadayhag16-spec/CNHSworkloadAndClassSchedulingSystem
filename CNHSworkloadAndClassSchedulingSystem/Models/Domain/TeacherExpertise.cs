namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class TeacherExpertise
    {
        public int TeacherExpertiseID { get; set; }

        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }

        public int SubjectID { get; set; }
        public Subject Subject { get; set; }
    }
}
