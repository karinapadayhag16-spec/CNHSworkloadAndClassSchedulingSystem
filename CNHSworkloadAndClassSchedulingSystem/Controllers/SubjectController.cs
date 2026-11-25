using Microsoft.AspNetCore.Mvc;
using CNHSworkloadAndClassSchedulingSystem.Models.ViewModels;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class SubjectController : Controller
    {
        private static List<SubjectViewModel> Subjects = new();

        public IActionResult Index()
        {
            return View(Subjects);
        }

        [HttpGet]
        public IActionResult AddSubject()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSubject(
            string subjectName,
            string department,
            string gradeLevel,
            int numberOfClasses)
        {
            Subjects.Add(new SubjectViewModel
            {
                SubjectName = subjectName,
                Department = department,
                GradeLevel = gradeLevel,
                NumberOfClasses = numberOfClasses
            });

            TempData["SuccessMessage"] =
                "Subject added successfully!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult EditSubject()
        {
            return View();
        }
    }
}