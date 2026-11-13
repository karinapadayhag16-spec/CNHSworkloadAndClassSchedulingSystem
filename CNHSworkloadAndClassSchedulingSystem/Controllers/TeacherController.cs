using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNHSworkloadAndClassSchedulingSystem.Data;
using CNHSworkloadAndClassSchedulingSystem.Models.Domain;
using CNHSworkloadAndClassSchedulingSystem.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using CNHSworkloadAndClassSchedulingSystem.Models.ViewModels;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeacherController(ApplicationDbContext context)
        {
            _context = context;
        }

        private void PopulateTeacherDropdowns(TeacherViewModel model)
        {
            model.Departments = Enum.GetValues<Department>()
                .Select(department => new SelectListItem
                {
                    Value = ((int)department).ToString(),
                    Text = department.ToString()
                })
                .ToList();
        }

        // GET: Teacher
        public async Task<IActionResult> Index()
        {
            var activeSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy =>
                    sy.Status == SchoolYearStatus.Active);

            if (activeSchoolYear == null)
            {
                TempData["ErrorMessage"] =
                    "There is no active school year.";

                return View(new List<Teacher>());
            }

            var teachers = await _context.Teachers
                .Include(t => t.SchoolYear)
                .Where(t =>
                    t.SchoolYearID == activeSchoolYear.SchoolYearId)
                .OrderBy(t => t.LastName)
                .ThenBy(t => t.FirstName)
                .ToListAsync();

            ViewBag.ActiveSchoolYear = activeSchoolYear.YearLabel;

            return View(teachers);
        }
        [HttpGet]
        public async Task<IActionResult> AddTeacher()
        {
            var activeSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy =>
                    sy.Status == SchoolYearStatus.Active);

            if (activeSchoolYear == null)
            {
                TempData["ErrorMessage"] =
                    "There is no active school year.";

                return RedirectToAction(nameof(Index));
            }

            var model = new TeacherViewModel
            {
                SchoolYearID = activeSchoolYear.SchoolYearId
            };

            // Pass the active school year label to the view
            ViewBag.ActiveSchoolYear = activeSchoolYear.YearLabel;

            PopulateTeacherDropdowns(model);

            return View(model);
        }
    }
}