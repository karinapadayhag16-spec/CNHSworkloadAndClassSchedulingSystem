using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class UserSettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddUsers()
        {
            return View();
        }

    }

}
