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

        // GET: Teacher
        public async Task<IActionResult> Index(int? termId)
        {
            var activeSchoolYear = await _context.SchoolYears
                .AsNoTracking()
                .FirstOrDefaultAsync(sy =>
                    sy.Status == SchoolYearStatus.Active);

            var pageModel = new TeacherIndexPageViewModel();

            if (activeSchoolYear == null)
            {
                TempData["ErrorMessage"] =
                    "There is no active school year.";

                return View(pageModel);
            }

            pageModel.ActiveSchoolYear = activeSchoolYear.YearLabel;

            // Retrieve the terms belonging to the active school year
            var terms = await _context.Terms
                .AsNoTracking()
                .Where(t => t.SchoolYearID == activeSchoolYear.SchoolYearId)
                .OrderBy(t => t.TermNumber)
                .ToListAsync();

            pageModel.Terms = terms.Select(t => new SelectListItem
            {
                Value = t.TermID.ToString(),
                Text = $"Term {t.TermNumber}"
            }).ToList();

            if (!terms.Any())
            {
                TempData["ErrorMessage"] =
                    "There are no terms configured for the active school year.";

                return View(pageModel);
            }

            // Use the selected term if it belongs to the active school year.
            // Otherwise, default to the first term.
            var selectedTerm = terms.FirstOrDefault(t =>
                t.TermID == termId);

            selectedTerm ??= terms.First();

            pageModel.SelectedTermID = selectedTerm.TermID;

            // Retrieve teachers assigned to the active school year
            var teachers = await _context.Teachers
                .AsNoTracking()
                .Where(t => t.SchoolYearAssignments.Any(a =>
                    a.SchoolYearID == activeSchoolYear.SchoolYearId))
                .Include(t => t.SchoolYearAssignments)
                    .ThenInclude(a => a.AncillaryDuties)
                        .ThenInclude(d => d.AncillaryDuty)
                .Include(t => t.SubjectExpertise)
                    .ThenInclude(te => te.Subject)
                .OrderBy(t => t.LastName)
                .ThenBy(t => t.FirstName)
                .ToListAsync();

            // Retrieve teaching schedules for the selected term.
            // Break periods are excluded.
            var schedules = await _context.Schedules
                .AsNoTracking()
                .Where(s =>
                    s.SubjectOffering.TermID == selectedTerm.TermID &&
                    !s.TimePeriod.IsBreak)
                .Select(s => new
                {
                    s.TeacherID,
                    s.TimePeriod.StartTime,
                    s.TimePeriod.EndTime
                })
                .ToListAsync();

            // Calculate each teacher's weekly teaching minutes
            var teachingMinutesByTeacher = schedules
                .GroupBy(s => s.TeacherID)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(s =>
                        (int)(s.EndTime - s.StartTime).TotalMinutes)
                );

            // Build the teacher rows for the Index page
            pageModel.Teachers = teachers.Select(teacher =>
            {
                var assignment = teacher.SchoolYearAssignments
                    .First(a =>
                        a.SchoolYearID == activeSchoolYear.SchoolYearId);

                var expertise = string.Join(", ",
                    teacher.SubjectExpertise
                        .Select(te => te.Subject.SubjectName)
                        .Distinct()
                        .OrderBy(name => name));

                return new TeacherIndexViewModel
                {
                    TeacherID = teacher.TeacherID,
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Department = assignment.Department,
                    Expertise = expertise,

                    ActualTeachingMinutes =
                    teachingMinutesByTeacher.TryGetValue(
                        teacher.TeacherID,
                        out var minutes)
                        ? minutes
                        : 0,

                    AncillaryDutyMinutes = assignment.AncillaryDuties
                        .Where(d =>
                            d.TermID == selectedTerm.TermID &&
                            d.CountsTowardWorkload)
                        .Sum(d => d.WeeklyMinutes),

                                        AssignedAncillaryDuties = assignment.AncillaryDuties
                        .Where(d => d.TermID == selectedTerm.TermID)
                        .Select(d => new AssignedAncillaryDutyViewModel
                        {
                            DutyName = d.AncillaryDuty.DutyName,
                            WeeklyMinutes = d.WeeklyMinutes,
                            CountsTowardWorkload = d.CountsTowardWorkload
                        })
                        .OrderBy(d => d.DutyName)
                        .ToList()
                };
            }).ToList();

            return View(pageModel);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTeacher(TeacherViewModel model)
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

            // Validate the selected department
            if (!model.Department.HasValue ||
                !Enum.IsDefined(typeof(Department), model.Department.Value))
            {
                ModelState.AddModelError(
                    nameof(model.Department),
                    "Please select a valid department.");
            }

            if (!ModelState.IsValid)
            {
                model.SchoolYearID = activeSchoolYear.SchoolYearId;

                ViewBag.ActiveSchoolYear = activeSchoolYear.YearLabel;

                PopulateTeacherDropdowns(model);

                return View(model);
            }

            // Create the permanent teacher record
            var teacher = new Teacher
            {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim()
            };

            // Create the teacher's assignment for the active school year
            var assignment = new TeacherSchoolYearAssignment
            {
                Teacher = teacher,
                SchoolYearID = activeSchoolYear.SchoolYearId,
                Department = model.Department.Value
            };

            _context.Teachers.Add(teacher);
            _context.TeacherSchoolYearAssignments.Add(assignment);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Teacher added successfully.";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> EditTeacher(int id)
        {
            // Get the active school year
            var activeSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy =>
                    sy.Status == SchoolYearStatus.Active);

            if (activeSchoolYear == null)
            {
                TempData["ErrorMessage"] =
                    "There is no active school year.";

                return RedirectToAction(nameof(Index));
            }

            // Find the teacher's assignment for the active school year
            var assignment = await _context.TeacherSchoolYearAssignments
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(a =>
                    a.TeacherID == id &&
                    a.SchoolYearID == activeSchoolYear.SchoolYearId);

            if (assignment == null)
            {
                return NotFound();
            }

            // Populate the edit form
            var model = new TeacherViewModel
            {
                TeacherID = assignment.Teacher.TeacherID,
                FirstName = assignment.Teacher.FirstName,
                LastName = assignment.Teacher.LastName,
                Department = assignment.Department,
                SchoolYearID = activeSchoolYear.SchoolYearId
            };

            ViewBag.ActiveSchoolYear = activeSchoolYear.YearLabel;

            PopulateTeacherDropdowns(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeacher(TeacherViewModel model)
        {
            // Get the active school year
            var activeSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy =>
                    sy.Status == SchoolYearStatus.Active);

            if (activeSchoolYear == null)
            {
                TempData["ErrorMessage"] =
                    "There is no active school year.";

                return RedirectToAction(nameof(Index));
            }

            // Validate the selected department
            if (model.Department == null ||
                !Enum.IsDefined(typeof(Department), model.Department.Value))
            {
                ModelState.AddModelError(
                    nameof(model.Department),
                    "Please select a valid department.");
            }

            // Find the teacher's assignment for the active school year
            var assignment = await _context.TeacherSchoolYearAssignments
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(a =>
                    a.TeacherID == model.TeacherID &&
                    a.SchoolYearID == activeSchoolYear.SchoolYearId);

            if (assignment == null)
            {
                return NotFound();
            }

            // If validation fails, reload the form data
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveSchoolYear = activeSchoolYear.YearLabel;

                PopulateTeacherDropdowns(model);

                return View(model);
            }

            // Update permanent teacher information
            assignment.Teacher.FirstName = model.FirstName.Trim();
            assignment.Teacher.LastName = model.LastName.Trim();

            // Update department for the active school year only
            assignment.Department = model.Department.Value;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Teacher information updated successfully.";

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> AssignAncillaryDuties(
            int id, int? termId)
        {
            // Get the active school year
            var activeSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy =>
                    sy.Status == SchoolYearStatus.Active);

            if (activeSchoolYear == null)
            {
                TempData["ErrorMessage"] =
                    "There is no active school year.";

                return RedirectToAction(nameof(Index));
            }

            // Find the teacher's assignment for the active school year
            var assignment = await _context.TeacherSchoolYearAssignments
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(a =>
                    a.TeacherID == id &&
                    a.SchoolYearID == activeSchoolYear.SchoolYearId);

            if (assignment == null)
            {
                return NotFound();
            }

            // Retrieve terms belonging to the active school year
            var terms = await _context.Terms
                .AsNoTracking()
                .Where(t =>
                    t.SchoolYearID == activeSchoolYear.SchoolYearId)
                .OrderBy(t => t.TermNumber)
                .ToListAsync();

            if (!terms.Any())
            {
                TempData["ErrorMessage"] =
                    "There are no terms configured for the active school year.";

                return RedirectToAction(nameof(Index));
            }

            // Use the selected term, or default to the first term
            var selectedTerm = terms.FirstOrDefault(t =>
                t.TermID == termId) ?? terms.First();

            // Get available ancillary duties
            var duties = await _context.AncillaryDuties
                .AsNoTracking()
                .OrderBy(d => d.DutyName)
                .ToListAsync();

            // Retrieve duties already assigned for the selected term
            var assignedDuties = await _context.TeacherAncillaryDuties
                .AsNoTracking()
                .Include(d => d.AncillaryDuty)
                .Where(d =>
                    d.TeacherAssignmentID == assignment.TeacherAssignmentID &&
                    d.TermID == selectedTerm.TermID)
                .OrderBy(d => d.AncillaryDuty.DutyName)
                .Select(d => new AssignedAncillaryDutyViewModel
                {
                    DutyName = d.AncillaryDuty.DutyName,
                    WeeklyMinutes = d.WeeklyMinutes,
                    CountsTowardWorkload = d.CountsTowardWorkload
                })
                .ToListAsync();

            // Prepare the assignment form
            var model = new TeacherAncillaryDutyViewModel
            {
                TeacherID = assignment.TeacherID,
                TeacherAssignmentID = assignment.TeacherAssignmentID,

                TeacherName =
                    $"{assignment.Teacher.FirstName} {assignment.Teacher.LastName}",

                SchoolYearLabel = activeSchoolYear.YearLabel ?? string.Empty,

                TermID = selectedTerm.TermID,

                AvailableTerms = terms.Select(t => new SelectListItem
                {
                    Value = t.TermID.ToString(),
                    Text = $"Term {t.TermNumber}",
                    Selected = t.TermID == selectedTerm.TermID
                }).ToList(),

                AvailableDuties = duties.Select(d => new SelectListItem
                {
                    Value = d.AncillaryDutyID.ToString(),
                    Text = d.FixedWeeklyMinutes.HasValue
                        ? $"{d.DutyName} ({d.FixedWeeklyMinutes.Value} min/week - Fixed)"
                        : $"{d.DutyName} (Flexible minutes)"
                }).ToList(),

                AssignedDuties = assignedDuties
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignAncillaryDuties(
            TeacherAncillaryDutyViewModel model)
        {
            // Get the active school year
            var activeSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy =>
                    sy.Status == SchoolYearStatus.Active);

            if (activeSchoolYear == null)
            {
                TempData["ErrorMessage"] =
                    "There is no active school year.";

                return RedirectToAction(nameof(Index));
            }

            // Find the teacher's assignment for the active school year
            var assignment = await _context.TeacherSchoolYearAssignments
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(a =>
                    a.TeacherID == model.TeacherID &&
                    a.SchoolYearID == activeSchoolYear.SchoolYearId);

            if (assignment == null)
            {
                return NotFound();
            }

            // Verify that the selected term belongs to the active school year
            var selectedTerm = await _context.Terms
                .FirstOrDefaultAsync(t =>
                    t.TermID == model.TermID &&
                    t.SchoolYearID == activeSchoolYear.SchoolYearId);

            if (selectedTerm == null)
            {
                ModelState.AddModelError(
                    nameof(model.TermID),
                    "Please select a valid term.");
            }

            // Get the selected ancillary duty
            var duty = await _context.AncillaryDuties
                .FirstOrDefaultAsync(d =>
                    d.AncillaryDutyID == model.AncillaryDutyID);

            if (duty == null)
            {
                ModelState.AddModelError(
                    nameof(model.AncillaryDutyID),
                    "Please select a valid ancillary duty.");
            }

            // Check for duplicate assignment within the selected term
            if (duty != null && selectedTerm != null)
            {
                var alreadyAssigned = await _context.TeacherAncillaryDuties
                    .AnyAsync(a =>
                        a.TeacherAssignmentID == assignment.TeacherAssignmentID &&
                        a.TermID == selectedTerm.TermID &&
                        a.AncillaryDutyID == duty.AncillaryDutyID);

                if (alreadyAssigned)
                {
                    ModelState.AddModelError(
                        nameof(model.AncillaryDutyID),
                        "This ancillary duty is already assigned to this teacher for the selected term.");
                }
            }

            // Validate flexible weekly minutes
            if (duty != null &&
                !duty.FixedWeeklyMinutes.HasValue &&
                (model.WeeklyMinutes < 0 || model.WeeklyMinutes > 1800))
            {
                ModelState.AddModelError(
                    nameof(model.WeeklyMinutes),
                    "Weekly minutes must be between 0 and 1800.");
            }

            // If validation fails, reload the form data
            if (!ModelState.IsValid)
            {
                model.TeacherAssignmentID = assignment.TeacherAssignmentID;

                model.TeacherName =
                    $"{assignment.Teacher.FirstName} {assignment.Teacher.LastName}";

                model.SchoolYearLabel =
                    activeSchoolYear.YearLabel ?? string.Empty;

                var terms = await _context.Terms
                    .AsNoTracking()
                    .Where(t =>
                        t.SchoolYearID == activeSchoolYear.SchoolYearId)
                    .OrderBy(t => t.TermNumber)
                    .ToListAsync();

                model.AvailableTerms = terms.Select(t => new SelectListItem
                {
                    Value = t.TermID.ToString(),
                    Text = $"Term {t.TermNumber}",
                    Selected = t.TermID == model.TermID
                }).ToList();

                var duties = await _context.AncillaryDuties
                    .AsNoTracking()
                    .OrderBy(d => d.DutyName)
                    .ToListAsync();

                model.AvailableDuties = duties.Select(d => new SelectListItem
                {
                    Value = d.AncillaryDutyID.ToString(),
                    Text = d.FixedWeeklyMinutes.HasValue
                        ? $"{d.DutyName} ({d.FixedWeeklyMinutes.Value} min/week - Fixed)"
                        : $"{d.DutyName} (Flexible minutes)"
                }).ToList();

                var assignedDuties = await _context.TeacherAncillaryDuties
                    .AsNoTracking()
                    .Include(d => d.AncillaryDuty)
                    .Where(d =>
                        d.TeacherAssignmentID == assignment.TeacherAssignmentID &&
                        d.TermID == model.TermID)
                    .OrderBy(d => d.AncillaryDuty.DutyName)
                    .Select(d => new AssignedAncillaryDutyViewModel
                    {
                        DutyName = d.AncillaryDuty.DutyName,
                        WeeklyMinutes = d.WeeklyMinutes,
                        CountsTowardWorkload = d.CountsTowardWorkload
                    })
                    .ToListAsync();

                model.AssignedDuties = assignedDuties;

                return View(model);
            }

            // Determine the actual weekly minutes
            // Fixed duties use the catalog value
            // Flexible duties use the value entered by the user
            int assignedMinutes = duty!.FixedWeeklyMinutes
                ?? model.WeeklyMinutes;

            // Create the teacher ancillary duty assignment
            var teacherDuty = new TeacherAncillaryDuty
            {
                TeacherAssignmentID = assignment.TeacherAssignmentID,
                TermID = selectedTerm!.TermID,
                AncillaryDutyID = duty.AncillaryDutyID,
                WeeklyMinutes = assignedMinutes,

                // Save the catalog rule as a historical snapshot
                CountsTowardWorkload = duty.CountsTowardWorkload
            };

            _context.TeacherAncillaryDuties.Add(teacherDuty);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Ancillary duty assigned successfully.";

            return RedirectToAction(
                nameof(AssignAncillaryDuties),
                new
                {
                    id = assignment.TeacherID,
                    termId = selectedTerm.TermID
                });
        }

    }
}