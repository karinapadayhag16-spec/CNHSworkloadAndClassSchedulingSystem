using Microsoft.AspNetCore.Mvc.Rendering;

namespace CNHSworkloadAndClassSchedulingSystem.Models.ViewModels
{
    public class TeacherIndexPageViewModel
    {
        public string? ActiveSchoolYear { get; set; }

        public int SelectedTermID { get; set; }

        public List<SelectListItem> Terms { get; set; } = new();

        public List<TeacherIndexViewModel> Teachers { get; set; } = new();
    }
}