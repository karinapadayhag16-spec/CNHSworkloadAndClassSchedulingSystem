
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNHSworkloadAndClassSchedulingSystem.Models.Domain
{
    public class Term
    {
        public int TermID { get; set; }

        public int TermNumber { get; set; }

        public int SchoolYearID { get; set; }

        [Required]
        [Range(1, 365)]
        public int InstructionalDays { get; set; }

        [ForeignKey(nameof(SchoolYearID))]
        public SchoolYear SchoolYear { get; set; } = null!;

        public ICollection<TimePeriod> TimePeriods { get; set; }
            = new List<TimePeriod>();
    }
}
