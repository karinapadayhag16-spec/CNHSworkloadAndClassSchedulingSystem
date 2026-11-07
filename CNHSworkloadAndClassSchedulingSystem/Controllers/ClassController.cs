using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNHSworkloadAndClassSchedulingSystem.Data;
using CNHSworkloadAndClassSchedulingSystem.Models.Domain;
using CNHSworkloadAndClassSchedulingSystem.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using CNHSworkloadAndClassSchedulingSystem.Models.ViewModels;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
    {
        public class Class : Controller
        {
            private readonly ApplicationDbContext _context;

            public Class(ApplicationDbContext context)
            {
                _context = context;
            }
            private async Task PopulateClassDropdowns(ClassViewModel model)
            {
                // School Years
                model.SchoolYears = await _context.SchoolYears
                    .OrderByDescending(sy => sy.YearLabel)
                    .Select(sy => new SelectListItem
                    {
                        Value = sy.SchoolYearId.ToString(),
                        Text = sy.YearLabel
                    })
                    .ToListAsync();

                // Departments
                model.Departments = Enum.GetValues<Department>()
                    .Select(d => new SelectListItem
                    {
                        Value = ((int)d).ToString(),
                        Text = d.ToString()
                    })
                    .ToList();

                // Grade Levels
                model.GradeLevels = Enumerable.Range(7, 6)
                    .Select(grade => new SelectListItem
                    {
                        Value = grade.ToString(),
                        Text = $"Grade {grade}"
                    })
                    .ToList();

                // Advisers for the selected School Year
                model.Advisers = await _context.Teachers
                    .Where(t => t.SchoolYearID == model.SchoolYearID)
                    .OrderBy(t => t.LastName)
                    .ThenBy(t => t.FirstName)
                    .Select(t => new SelectListItem
                    {
                        Value = t.TeacherID.ToString(),
                        Text = t.LastName + ", " + t.FirstName
                    })
                    .ToListAsync();
            }
            private async Task<ClassSection?> GetPreviousClassSection(ClassViewModel model,SchoolYear activeSchoolYear)
                {
                    // Extract the starting year from the active school year label
                    string[] years = activeSchoolYear.YearLabel!.Split('-');

                    if (years.Length != 2 ||
                        !int.TryParse(years[0], out int currentStartYear))
                    {
                        return null;
                    }

                    // Determine the previous school year label
                    string previousYearLabel =
                        $"{currentStartYear - 1}-{currentStartYear}";

                    // Find the previous school year
                    var previousSchoolYear = await _context.SchoolYears
                        .FirstOrDefaultAsync(sy =>
                            sy.YearLabel == previousYearLabel);

                    if (previousSchoolYear == null)
                        return null;

                    // Find the matching previous class section
                    return await _context.ClassSections
                        .Include(cs => cs.Adviser)
                        .FirstOrDefaultAsync(cs =>
                            cs.SchoolYearID == previousSchoolYear.SchoolYearId &&
                            cs.Department == model.Department &&
                            cs.GradeLevel == model.GradeLevel &&
                            cs.SectionName == model.SectionName);
                }

        // GET: Classes
        public async Task<IActionResult> Index()
            {
                var activeSchoolYear = await _context.SchoolYears
                    .FirstOrDefaultAsync(sy =>
                        sy.Status == SchoolYearStatus.Active);

                if (activeSchoolYear == null)
                {
                    TempData["ErrorMessage"] =
                        "There is no active school year.";

                    return View(new List<ClassSection>());
                }

                var classes = await _context.ClassSections
                    .Include(cs => cs.Adviser)
                    .Include(cs => cs.SchoolYear)
                    .Where(cs =>
                        cs.SchoolYearID == activeSchoolYear.SchoolYearId)
                    .OrderBy(cs => cs.Department)
                    .ThenBy(cs => cs.GradeLevel)
                    .ThenBy(cs => cs.SectionName)
                    .ToListAsync();

                ViewBag.ActiveSchoolYear = activeSchoolYear.YearLabel;

                return View(classes);
            }
        [HttpGet]
        public async Task<IActionResult> GetPreviousAdviser(
            Department department, int gradeLevel, string sectionName)
        {
            var activeSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy =>
                    sy.Status == SchoolYearStatus.Active);

            if (activeSchoolYear == null ||
                string.IsNullOrWhiteSpace(activeSchoolYear.YearLabel))
            {
                return Json(new { found = false });
            }

            string[] years = activeSchoolYear.YearLabel.Split('-');

            if (years.Length != 2 ||
                !int.TryParse(years[0], out int currentStartYear))
            {
                return Json(new { found = false });
            }

            string previousYearLabel =
                $"{currentStartYear - 1}-{currentStartYear}";

            var previousSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy =>
                    sy.YearLabel == previousYearLabel);

            if (previousSchoolYear == null)
                return Json(new { found = false });

            var previousClass = await _context.ClassSections
                .Include(cs => cs.Adviser)
                .FirstOrDefaultAsync(cs =>
                    cs.SchoolYearID == previousSchoolYear.SchoolYearId &&
                    cs.Department == department &&
                    cs.GradeLevel == gradeLevel &&
                    cs.SectionName == sectionName.Trim());

            if (previousClass?.Adviser == null)
                return Json(new { found = false });

            return Json(new
            {
                found = true,
                adviserID = previousClass.AdviserID,
                adviserName = $"{previousClass.Adviser.FirstName} {previousClass.Adviser.LastName}",
                previousSectionID = previousClass.ClassSectionID,
                previousYear = previousSchoolYear.YearLabel
            });
        }
        [HttpGet]
        public async Task<IActionResult> AddClass()
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

            var model = new ClassViewModel
            {
                SchoolYearID = activeSchoolYear.SchoolYearId
            };

            await PopulateClassDropdowns(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClass(ClassViewModel model)
        {
            // Get the active school year
            var activeSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy =>
                    sy.Status == SchoolYearStatus.Active);
            var previousClass = await GetPreviousClassSection(
                model,
                activeSchoolYear);
            if (activeSchoolYear == null)
            {
                TempData["ErrorMessage"] =
                    "There is no active school year.";

                return RedirectToAction(nameof(Index));
            }

            // Only allow adding classes to the active school year
            model.SchoolYearID = activeSchoolYear.SchoolYearId;

            if (model.AdviserID.HasValue)
            {
                var adviserExists = await _context.Teachers
                    .AnyAsync(t =>
                        t.TeacherID == model.AdviserID.Value &&
                        t.SchoolYearID == activeSchoolYear.SchoolYearId);

                if (!adviserExists)
                {
                    ModelState.AddModelError(
                        "AdviserID",
                        "Please select a valid adviser for the active school year.");
                }
            }

            // Check for duplicate class sections
            if (ModelState.IsValid)
            {
                string sectionName = model.SectionName.Trim();

                bool sectionExists = await _context.ClassSections
                    .AnyAsync(cs =>
                        cs.SchoolYearID == activeSchoolYear.SchoolYearId &&
                        cs.Department == model.Department &&
                        cs.GradeLevel == model.GradeLevel &&
                        cs.SectionName.ToLower() == sectionName.ToLower());

                if (sectionExists)
                {
                    ModelState.AddModelError(
                        "SectionName",
                        "This section already exists for the selected grade level and department.");
                }
            }

            // If validation fails, reload dropdowns
            if (!ModelState.IsValid)
            {
                await PopulateClassDropdowns(model);
                return View(model);
            }

            // Create the class section
            var classSection = new ClassSection
            {
                Department = model.Department.Value,
                GradeLevel = model.GradeLevel,
                SectionName = model.SectionName.Trim(),
                AdviserID = model.AdviserID,
                SchoolYearID = activeSchoolYear.SchoolYearId,
                PreviousClassSectionID = previousClass?.ClassSectionID
            };

            _context.ClassSections.Add(classSection);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Class section successfully added.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditClass(int id)
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

            // Retrieve only the selected class section
            var classSection = await _context.ClassSections
                .FirstOrDefaultAsync(cs =>
                    cs.ClassSectionID == id);

            if (classSection == null)
            {
                return NotFound();
            }

            // Prevent editing classes from previous school years
            if (classSection.SchoolYearID != activeSchoolYear.SchoolYearId)
            {
                TempData["ErrorMessage"] =
                    "Only class sections belonging to the active school year can be edited.";

                return RedirectToAction(nameof(Index));
            }

            // Transfer existing class information to the ViewModel
            var model = new ClassViewModel
            {
                ClassSectionID = classSection.ClassSectionID,
                SchoolYearID = classSection.SchoolYearID,
                Department = classSection.Department,
                GradeLevel = classSection.GradeLevel,
                SectionName = classSection.SectionName,
                AdviserID = classSection.AdviserID
            };

            // Populate dropdowns
            await PopulateClassDropdowns(model);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClass(ClassViewModel model)
        {
            // Get the active school year
            var activeSchoolYear = await _context.SchoolYears
                .FirstOrDefaultAsync(sy =>
                    sy.Status == SchoolYearStatus.Active);

            if (activeSchoolYear == null)
            {
                TempData["ErrorMessage"] = "There is no active school year.";
                return RedirectToAction(nameof(Index));
            }

            // Retrieve only the selected class record
            var classSection = await _context.ClassSections
                .FirstOrDefaultAsync(cs =>
                    cs.ClassSectionID == model.ClassSectionID);

            if (classSection == null)
            {
                return NotFound();
            }

            // Prevent editing historical records
            if (classSection.SchoolYearID != activeSchoolYear.SchoolYearId)
            {
                TempData["ErrorMessage"] =
                    "Only classes belonging to the active school year can be edited.";

                return RedirectToAction(nameof(Index));
            }

            model.SchoolYearID = classSection.SchoolYearID;

            // Validate adviser if one is selected
            if (model.AdviserID.HasValue)
            {
                bool adviserExists = await _context.Teachers
                    .AnyAsync(t =>
                        t.TeacherID == model.AdviserID.Value &&
                        t.SchoolYearID == classSection.SchoolYearID);

                if (!adviserExists)
                {
                    ModelState.AddModelError(
                        "AdviserID",
                        "The selected adviser is not valid for this school year.");
                }
            }

            // Check for duplicate class sections
            if (ModelState.IsValid)
            {
                string sectionName = model.SectionName.Trim();

                bool duplicateExists = await _context.ClassSections
                    .AnyAsync(cs =>
                        cs.ClassSectionID != model.ClassSectionID &&
                        cs.SchoolYearID == classSection.SchoolYearID &&
                        cs.Department == model.Department &&
                        cs.GradeLevel == model.GradeLevel &&
                        cs.SectionName.ToLower() == sectionName.ToLower());

                if (duplicateExists)
                {
                    ModelState.AddModelError(
                        "SectionName",
                        "This class section already exists.");
                }
            }

            // Return the form if validation fails
            if (!ModelState.IsValid)
            {
                await PopulateClassDropdowns(model);
                return View(model);
            }

            // Update only the selected class record
            classSection.Department = model.Department.Value;
            classSection.GradeLevel = model.GradeLevel;
            classSection.SectionName = model.SectionName.Trim();
            classSection.AdviserID = model.AdviserID;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Class section successfully updated.";

            return RedirectToAction(nameof(Index));
        }
    }
}
