using CNHSworkloadAndClassSchedulingSystem.Models.Domain;
using CNHSworkloadAndClassSchedulingSystem.Models.Enums;

public class Teacher
{
    public int TeacherID { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Department Department { get; set; }

    public bool IsArchived { get; set; } = false;

    public ICollection<TeacherExpertise> SubjectExpertise { get; set; }
        = new List<TeacherExpertise>();
}