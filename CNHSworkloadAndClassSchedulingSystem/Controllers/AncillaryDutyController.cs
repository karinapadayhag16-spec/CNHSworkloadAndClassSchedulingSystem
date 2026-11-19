using CNHSworkloadAndClassSchedulingSystem.Data;
using CNHSworkloadAndClassSchedulingSystem.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class AncillaryDutyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AncillaryDutyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var duties = await _context.AncillaryDuties
                .AsNoTracking()
                .OrderBy(d => d.DutyName)
                .ToListAsync();

            return View(duties);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AncillaryDuty model)
        {
            // Validate duty name
            if (string.IsNullOrWhiteSpace(model.DutyName))
            {
                ModelState.AddModelError(
                    nameof(model.DutyName),
                    "Duty name is required.");
            }

            // Validate fixed weekly minutes
            if (model.FixedWeeklyMinutes.HasValue &&
                (model.FixedWeeklyMinutes.Value < 0 ||
                 model.FixedWeeklyMinutes.Value > 1800))
            {
                ModelState.AddModelError(
                    nameof(model.FixedWeeklyMinutes),
                    "Fixed weekly minutes must be between 0 and 1800.");
            }

            // Check for duplicate duty names
            var dutyName = model.DutyName?.Trim();

            if (!string.IsNullOrWhiteSpace(dutyName))
            {
                var exists = await _context.AncillaryDuties
                    .AnyAsync(d => d.DutyName == dutyName);

                if (exists)
                {
                    ModelState.AddModelError(
                        nameof(model.DutyName),
                        "This ancillary duty already exists.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Create the ancillary duty
            var duty = new AncillaryDuty
            {
                DutyName = dutyName!,
                FixedWeeklyMinutes = model.FixedWeeklyMinutes,
                CountsTowardWorkload = model.CountsTowardWorkload
            };

            _context.AncillaryDuties.Add(duty);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Ancillary duty added successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var duty = await _context.AncillaryDuties
                .FirstOrDefaultAsync(d => d.AncillaryDutyID == id);

            if (duty == null)
            {
                return NotFound();
            }

            return View(duty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AncillaryDuty model)
        {
            // Find the existing ancillary duty
            var duty = await _context.AncillaryDuties
                .FirstOrDefaultAsync(d =>
                    d.AncillaryDutyID == model.AncillaryDutyID);

            if (duty == null)
            {
                return NotFound();
            }

            // Validate duty name
            if (string.IsNullOrWhiteSpace(model.DutyName))
            {
                ModelState.AddModelError(
                    nameof(model.DutyName),
                    "Duty name is required.");
            }

            // Validate fixed weekly minutes
            if (model.FixedWeeklyMinutes.HasValue &&
                (model.FixedWeeklyMinutes.Value < 0 ||
                 model.FixedWeeklyMinutes.Value > 1800))
            {
                ModelState.AddModelError(
                    nameof(model.FixedWeeklyMinutes),
                    "Fixed weekly minutes must be between 0 and 1800.");
            }

            // Check for duplicate duty names, excluding the current duty
            var dutyName = model.DutyName?.Trim();

            if (!string.IsNullOrWhiteSpace(dutyName))
            {
                var exists = await _context.AncillaryDuties
                    .AnyAsync(d =>
                        d.DutyName == dutyName &&
                        d.AncillaryDutyID != model.AncillaryDutyID);

                if (exists)
                {
                    ModelState.AddModelError(
                        nameof(model.DutyName),
                        "This ancillary duty already exists.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Update the catalog record
            duty.DutyName = dutyName!;
            duty.FixedWeeklyMinutes = model.FixedWeeklyMinutes;
            duty.CountsTowardWorkload = model.CountsTowardWorkload;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Ancillary duty updated successfully.";

            return RedirectToAction(nameof(Index));
        }



    }
}
