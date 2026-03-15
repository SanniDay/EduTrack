using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
