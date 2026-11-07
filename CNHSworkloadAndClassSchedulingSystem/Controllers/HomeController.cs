using CNHSworkloadAndClassSchedulingSystem.Data;
using CNHSworkloadAndClassSchedulingSystem.Models;
using CNHSworkloadAndClassSchedulingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Classes
            ViewBag.JhsClasses = await _context.ClassSections
                .CountAsync(c => c.Department == Models.Enums.Department.JHS);

            ViewBag.ShsClasses = await _context.ClassSections
                .CountAsync(c => c.Department == Models.Enums.Department.SHS);

            ViewBag.TotalClasses =
                ViewBag.JhsClasses + ViewBag.ShsClasses;

            // Teachers
            ViewBag.TotalTeachers =
                await _context.Teachers.CountAsync();

            // Users
            ViewBag.TotalUsers =
                await _context.ApplicationUsers.CountAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId =
                        Activity.Current?.Id ??
                        HttpContext.TraceIdentifier
                });
        }
    }
}