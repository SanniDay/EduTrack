using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    public class TeacherClassController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
