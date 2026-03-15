using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    public class StudentClassController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
