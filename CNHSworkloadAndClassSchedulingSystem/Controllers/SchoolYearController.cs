using CNHSworkloadAndClassSchedulingSystem.Data;
using CNHSworkloadAndClassSchedulingSystem.Models;
using CNHSworkloadAndClassSchedulingSystem.Models.Domain;
using CNHSworkloadAndClassSchedulingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class SchoolYearController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchoolYearController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var schoolYears = await _context.SchoolYears
       .OrderByDescending(sy => sy.SchoolYearId)
       .ToListAsync();

            return View(schoolYears);
        }

        [HttpGet]
        public IActionResult AddSchoolYear()
        {
            var viewModel = new AddSchoolYearViewModel
            {
                StartYear = DateTime.Now.Year,
                EndYear = DateTime.Now.Year + 1
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSchoolYear(
            AddSchoolYearViewModel viewModel)
        {
            if (viewModel.EndYear != viewModel.StartYear + 1)
            {
                ModelState.AddModelError(
                    nameof(viewModel.EndYear),
                    "The end year must be exactly one year after the start year.");
            }

            string yearLabel =
                $"{viewModel.StartYear}-{viewModel.EndYear}";

            if (await _context.SchoolYears
                .AnyAsync(s => s.YearLabel == yearLabel))
            {
                ModelState.AddModelError(
                    nameof(viewModel.StartYear),
                    "This school year already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var schoolYear = new SchoolYear
            {
                YearLabel = yearLabel,
                IsCurrent = viewModel.IsCurrent
            };

            _context.SchoolYears.Add(schoolYear);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
