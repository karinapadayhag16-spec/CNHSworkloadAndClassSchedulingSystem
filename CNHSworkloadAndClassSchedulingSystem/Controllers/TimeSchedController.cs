
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNHSworkloadAndClassSchedulingSystem.Data;
using CNHSworkloadAndClassSchedulingSystem.Models.Domain;
using CNHSworkloadAndClassSchedulingSystem.Models.ViewModels;
using CNHSworkloadAndClassSchedulingSystem.Models.Enums;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class TimeSchedController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimeSchedController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TimeSched
        public async Task<IActionResult> Index()
        {
            var schedules = await _context.TimeSchedules
                .Include(ts => ts.TimePeriods)
                .OrderBy(ts => ts.ScheduleName)
                .ThenByDescending(ts => ts.VersionNumber)
                .ToListAsync();

            return View(schedules);
        }

        // GET: TimeSched/AddSchedule
        [HttpGet]
        public IActionResult AddSchedule()
        {
            var model = new TimeScheduleViewModel();

            model.TimePeriods.Add(new TimePeriodViewModel
            {
                PeriodNumber = 1
            });

            return View(model);
        }

        // POST: TimeSched/AddSchedule
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSchedule(
            TimeScheduleViewModel model)
        {
            if (model.TimePeriods == null ||
                !model.TimePeriods.Any())
            {
                ModelState.AddModelError(
                    nameof(model.TimePeriods),
                    "At least one time period is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validate periods
            var periods = model.TimePeriods
                .OrderBy(p => p.StartTime)
                .ToList();

            for (int i = 0; i < periods.Count; i++)
            {
                if (periods[i].StartTime >= periods[i].EndTime)
                {
                    ModelState.AddModelError(
                        nameof(model.TimePeriods),
                        $"Period {periods[i].PeriodNumber}: " +
                        "End time must be later than start time.");
                }

                if (i > 0 &&
                    periods[i].StartTime < periods[i - 1].EndTime)
                {
                    ModelState.AddModelError(
                        nameof(model.TimePeriods),
                        "Time periods must not overlap.");
                }
            }

            if (periods.Select(p => p.PeriodNumber).Distinct().Count()
                != periods.Count)
            {
                ModelState.AddModelError(
                    nameof(model.TimePeriods),
                    "Period numbers must be unique.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Create the first schedule version
            var schedule = new TimeSchedule
            {
                ScheduleName = model.ScheduleName,
                Description = model.Description,
                VersionNumber = 1,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var period in periods.OrderBy(p => p.PeriodNumber))
            {
                schedule.TimePeriods.Add(new TimePeriod
                {
                    PeriodNumber = period.PeriodNumber,
                    StartTime = period.StartTime,
                    EndTime = period.EndTime,
                    IsBreak = period.IsBreak
                });
            }

            _context.TimeSchedules.Add(schedule);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Time schedule created successfully.";

            return RedirectToAction(nameof(Index));
        }
        // GET: TimeSched/ViewSchedule/5
        [HttpGet]
        public async Task<IActionResult> ViewSchedule(int id)
        {
            var schedule = await _context.TimeSchedules
                .Include(ts => ts.TimePeriods)
                .FirstOrDefaultAsync(ts => ts.TimeScheduleId == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }
        // GET: TimeSched/EditSchedule/5
        [HttpGet]
        public async Task<IActionResult> EditSchedule(int id)
        {
            var schedule = await _context.TimeSchedules
                .Include(ts => ts.TimePeriods)
                .FirstOrDefaultAsync(ts => ts.TimeScheduleId == id);

            if (schedule == null)
            {
                return NotFound();
            }

            // Archived schedules cannot be edited.
            if (schedule.IsArchived)
            {
                TempData["ErrorMessage"] =
                    "Archived class time schedules cannot be edited.";

                return RedirectToAction(nameof(Index));
            }

            var model = new TimeScheduleViewModel
            {
                ScheduleName = schedule.ScheduleName,
                Description = schedule.Description,

                TimePeriods = schedule.TimePeriods
                    .OrderBy(tp => tp.PeriodNumber)
                    .Select(tp => new TimePeriodViewModel
                    {
                        PeriodNumber = tp.PeriodNumber,
                        StartTime = tp.StartTime,
                        EndTime = tp.EndTime,
                        IsBreak = tp.IsBreak
                    })
                    .ToList()
            };
            ViewBag.TimeScheduleId = schedule.TimeScheduleId;
            return View(model);
        }

        // POST: TimeSched/EditSchedule/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSchedule(
            int id,
            TimeScheduleViewModel model)
        {
            ViewBag.TimeScheduleId = id;

            // Check whether the original schedule exists.
            var oldSchedule = await _context.TimeSchedules
                .Include(ts => ts.TimePeriods)
                .FirstOrDefaultAsync(ts => ts.TimeScheduleId == id);

            if (oldSchedule == null)
            {
                return NotFound();
            }

            if (oldSchedule.IsArchived)
            {
                TempData["ErrorMessage"] =
                    "Archived class time schedules cannot be edited.";

                return RedirectToAction(nameof(Index));
            }

            // Validate that at least one period exists.
            if (model.TimePeriods == null ||
                !model.TimePeriods.Any())
            {
                ModelState.AddModelError(
                    nameof(model.TimePeriods),
                    "At least one time period is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Sort periods by start time for validation.
            var periods = model.TimePeriods
                .OrderBy(p => p.StartTime)
                .ToList();

            // Validate period times.
            for (int i = 0; i < periods.Count; i++)
            {
                if (periods[i].StartTime >= periods[i].EndTime)
                {
                    ModelState.AddModelError(
                        nameof(model.TimePeriods),
                        $"Period {periods[i].PeriodNumber}: " +
                        "End time must be later than start time.");
                }

                if (i > 0 &&
                    periods[i].StartTime < periods[i - 1].EndTime)
                {
                    ModelState.AddModelError(
                        nameof(model.TimePeriods),
                        "Time periods must not overlap.");
                }
            }

            // Validate unique period numbers.
            if (periods.Select(p => p.PeriodNumber).Distinct().Count()
                != periods.Count)
            {
                ModelState.AddModelError(
                    nameof(model.TimePeriods),
                    "Period numbers must be unique.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Begin database transaction.
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Get all ClassSectionSchedule assignments using
                // the old version in the active school year.
                var activeAssignments = await _context.ClassSectionSchedules
                    .Include(css => css.Term)
                        .ThenInclude(t => t.SchoolYear)
                    .Include(css => css.ClassSection)
                    .Where(css =>
                        css.TimeScheduleId == id &&
                        css.Term.SchoolYear.Status == SchoolYearStatus.Active &&
                        css.ClassSection.SchoolYearID == css.Term.SchoolYearID)
                    .ToListAsync();

                // Get the class section and term combinations.
                var assignmentKeys = activeAssignments
                    .Select(css => (
                        css.ClassSectionID,
                        css.TermID))
                    .ToHashSet();

                var activeTermIds = activeAssignments
                    .Select(css => css.TermID)
                    .Distinct()
                    .ToList();

                var activeClassSectionIds = activeAssignments
                    .Select(css => css.ClassSectionID)
                    .Distinct()
                    .ToList();

                // Get the old time period IDs.
                var oldPeriodIds = oldSchedule.TimePeriods
                    .Select(tp => tp.TimePeriodId)
                    .ToList();

                // Get timetable entries belonging to the affected
                // class section and term assignments.
                var timetableEntries = await _context.Schedules
                    .Include(s => s.SubjectOffering)
                    .Where(s =>
                        activeTermIds.Contains(s.SubjectOffering.TermID) &&
                        activeClassSectionIds.Contains(
                            s.SubjectOffering.ClassSectionID) &&
                        oldPeriodIds.Contains(s.TimePeriodID))
                    .ToListAsync();

                // Keep only entries matching an actual active assignment.
                timetableEntries = timetableEntries
                    .Where(s => assignmentKeys.Contains((
                        s.SubjectOffering.ClassSectionID,
                        s.SubjectOffering.TermID)))
                    .ToList();

                // Map old periods using their PeriodNumber.
                var oldPeriodNumbers = oldSchedule.TimePeriods
                    .ToDictionary(
                        tp => tp.TimePeriodId,
                        tp => tp.PeriodNumber);

                var newPeriodNumbers = periods
                    .ToDictionary(
                        p => p.PeriodNumber,
                        p => p);

                // Ensure that no period currently used by a class
                // is removed from the new version.
                foreach (var entry in timetableEntries)
                {
                    int oldPeriodNumber =
                        oldPeriodNumbers[entry.TimePeriodID];

                    if (!newPeriodNumbers.TryGetValue(
                            oldPeriodNumber, out var newPeriod))
                    {
                        ModelState.AddModelError(
                            nameof(model.TimePeriods),
                            $"Period {oldPeriodNumber} cannot be removed " +
                            "because it is currently used in a class timetable.");

                        await transaction.RollbackAsync();
                        return View(model);
                    }

                    // A class cannot be assigned to a break period.
                    if (newPeriod.IsBreak)
                    {
                        ModelState.AddModelError(
                            nameof(model.TimePeriods),
                            $"Period {oldPeriodNumber} cannot be changed " +
                            "to a break because it is currently used " +
                            "in a class timetable.");

                        await transaction.RollbackAsync();
                        return View(model);
                    }
                }

                // Create the new schedule version.
                var newSchedule = new TimeSchedule
                {
                    ScheduleName = model.ScheduleName,
                    Description = model.Description,

                    VersionNumber = oldSchedule.VersionNumber + 1,

                    BasedOnTimeScheduleId = oldSchedule.TimeScheduleId,

                    IsArchived = false,

                    ChangeReason = model.ChangeReason,

                    CreatedAt = DateTime.UtcNow
                };

                // Copy the edited periods into the new version.
                foreach (var period in periods.OrderBy(p => p.PeriodNumber))
                {
                    newSchedule.TimePeriods.Add(new TimePeriod
                    {
                        PeriodNumber = period.PeriodNumber,
                        StartTime = period.StartTime,
                        EndTime = period.EndTime,
                        IsBreak = period.IsBreak
                    });
                }

                _context.TimeSchedules.Add(newSchedule);

                // Save first to generate the new schedule and period IDs.
                await _context.SaveChangesAsync();

                // Create a mapping:
                // Old TimePeriod ID -> New TimePeriod ID
                var newPeriodsByNumber = newSchedule.TimePeriods
                    .ToDictionary(
                        tp => tp.PeriodNumber,
                        tp => tp.TimePeriodId);

                foreach (var entry in timetableEntries)
                {
                    int oldPeriodNumber =
                        oldPeriodNumbers[entry.TimePeriodID];

                    entry.TimePeriodID =
                        newPeriodsByNumber[oldPeriodNumber];
                }

                // Automatically assign the new version to all affected
                // classes in the active school year.
                foreach (var assignment in activeAssignments)
                {
                    assignment.TimeScheduleId =
                        newSchedule.TimeScheduleId;
                }

                // Archive the previous version.
                oldSchedule.IsArchived = true;

                // Save timetable changes, assignment updates,
                // and archive the previous version.
                await _context.SaveChangesAsync();

                // Commit everything together.
                await transaction.CommitAsync();

                TempData["SuccessMessage"] =
                    $"Class Time Schedule updated successfully. " +
                    $"Version {newSchedule.VersionNumber} was created.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        // GET: TimeSched/EditDetails/5
        [HttpGet]
        public async Task<IActionResult> EditDetails(int id)
        {
            var schedule = await _context.TimeSchedules
                .FirstOrDefaultAsync(ts => ts.TimeScheduleId == id);

            if (schedule == null)
                return NotFound();

            if (schedule.IsArchived)
            {
                TempData["ErrorMessage"] =
                    "Archived schedules cannot be edited.";

                return RedirectToAction(nameof(Index));
            }

            var model = new TimeScheduleViewModel
            {
                ScheduleName = schedule.ScheduleName,
                Description = schedule.Description
            };

            ViewBag.TimeScheduleId = schedule.TimeScheduleId;

            return View(model);
        }


        // POST: TimeSched/EditDetails/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetails(
            int id,
            TimeScheduleViewModel model)
        {
            var schedule = await _context.TimeSchedules
                .FirstOrDefaultAsync(ts => ts.TimeScheduleId == id);

            if (schedule == null)
                return NotFound();

            if (schedule.IsArchived)
            {
                TempData["ErrorMessage"] =
                    "Archived schedules cannot be edited.";

                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                ViewBag.TimeScheduleId = id;
                return View(model);
            }

            // Update only the schedule details.
            schedule.ScheduleName = model.ScheduleName;
            schedule.Description = model.Description;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Class Time Schedule details updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        public IActionResult ArchivedSchedule()
        {
            var archivedSchedules = _context.TimeSchedules
                .Where(x => x.IsArchived)
                .ToList();

            return View(archivedSchedules);
        }
    }
}
