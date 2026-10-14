
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNHSworkloadAndClassSchedulingSystem.Data;

namespace CNHSworkloadAndClassSchedulingSystem.ViewComponents
{
    public class CurrentSchoolYearViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CurrentSchoolYearViewComponent(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentSchoolYear = await _context.SchoolYears
                .Where(sy => sy.IsCurrent)
                .Select(sy => sy.YearLabel)
                .FirstOrDefaultAsync();

            return Content(currentSchoolYear ?? "No Current School Year");
        }
    }
}
