using CNHSworkloadAndClassSchedulingSystem.Data;
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
                .ToListAsync();

            schoolYears = schoolYears
                .OrderByDescending(s =>
                    int.Parse(s.YearLabel.Split('-')[0]))
                .ToList();

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

            if (viewModel.IsCurrent)
            {
                var currentSchoolYears = await _context.SchoolYears
                    .Where(s => s.IsCurrent)
                    .ToListAsync();

                foreach (var sy in currentSchoolYears)
                {
                    sy.IsCurrent = false;
                }
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

        [HttpGet]
        public async Task<IActionResult> EditSchoolYear(int id)
        {
            var schoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(s => s.SchoolYearId == id);

            if (schoolYear == null)
            {
                return NotFound();
            }

            var years = schoolYear.YearLabel.Split('-');

            var viewModel = new AddSchoolYearViewModel
            {
                StartYear = int.Parse(years[0]),
                EndYear = int.Parse(years[1]),
                IsCurrent = schoolYear.IsCurrent
            };

            ViewBag.SchoolYearId = schoolYear.SchoolYearId;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSchoolYear(
            int id,
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
                .AnyAsync(s =>
                    s.YearLabel == yearLabel &&
                    s.SchoolYearId != id))
            {
                ModelState.AddModelError(
                    nameof(viewModel.StartYear),
                    "This school year already exists.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.SchoolYearId = id;
                return View(viewModel);
            }

            var schoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(s => s.SchoolYearId == id);

            if (schoolYear == null)
            {
                return NotFound();
            }

            if (viewModel.IsCurrent)
            {
                var currentSchoolYears = await _context.SchoolYears
                    .Where(s =>
                        s.IsCurrent &&
                        s.SchoolYearId != id)
                    .ToListAsync();

                foreach (var sy in currentSchoolYears)
                {
                    sy.IsCurrent = false;
                }
            }

            schoolYear.YearLabel = yearLabel;
            schoolYear.IsCurrent = viewModel.IsCurrent;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}