using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    public class ClassController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
