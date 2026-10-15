using CNHSworkloadAndClassSchedulingSystem.Data;
using CNHSworkloadAndClassSchedulingSystem.Models.Domain;
using CNHSworkloadAndClassSchedulingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class TermController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TermController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var terms = await _context.Terms
                .Include(t => t.SchoolYear)
                .OrderByDescending(t => t.SchoolYearID)
                .ThenBy(t => t.TermNumber)
                .ToListAsync();

            return View(terms);
        }

        [HttpGet]
        public async Task<IActionResult> AddTerm()
        {
            var currentSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy => sy.IsCurrent);

            if (currentSchoolYear == null)
            {
                TempData["ErrorMessage"] =
                    "Please set a current school year first.";

                return RedirectToAction("Index", "SchoolYear");
            }

            int nextTermNumber = (await _context.Terms
                .Where(t => t.SchoolYearID ==
                            currentSchoolYear.SchoolYearId)
                .Select(t => (int?)t.TermNumber)
                .MaxAsync() ?? 0) + 1;

            var viewModel = new AddTermViewModel
            {
                SchoolYearLabel = currentSchoolYear.YearLabel,
                TermNumber = nextTermNumber
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTerm(AddTermViewModel viewModel)
        {
            var currentSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy => sy.IsCurrent);

            if (currentSchoolYear == null)
            {
                TempData["ErrorMessage"] =
                    "Please set a current school year first.";

                return RedirectToAction("Index", "SchoolYear");
            }

            bool termExists = await _context.Terms.AnyAsync(t =>
                t.SchoolYearID == currentSchoolYear.SchoolYearId &&
                t.TermNumber == viewModel.TermNumber);

            if (termExists)
            {
                ModelState.AddModelError(
                    nameof(viewModel.TermNumber),
                    "This term number already exists for the current school year.");
            }

            if (!ModelState.IsValid)
            {
                viewModel.SchoolYearLabel =
                    currentSchoolYear.YearLabel;

                return View(viewModel);
            }

            var term = new Term
            {
                SchoolYearID = currentSchoolYear.SchoolYearId,
                TermNumber = viewModel.TermNumber,
                InstructionalDays = viewModel.InstructionalDays
            };

            _context.Terms.Add(term);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Term {term.TermNumber} was added successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditTerm(int id)
        {
            var term = await _context.Terms
                .Include(t => t.SchoolYear)
                .FirstOrDefaultAsync(t => t.TermID == id);

            if (term == null)
            {
                return NotFound();
            }

            var viewModel = new AddTermViewModel
            {
                SchoolYearLabel = term.SchoolYear.YearLabel,
                TermNumber = term.TermNumber,
                InstructionalDays = term.InstructionalDays
            };

            ViewBag.TermID = term.TermID;

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTerm(
    int id,
    AddTermViewModel viewModel)
        {
            var term = await _context.Terms
                .FirstOrDefaultAsync(t => t.TermID == id);

            if (term == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            term.InstructionalDays = viewModel.InstructionalDays;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Term updated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
