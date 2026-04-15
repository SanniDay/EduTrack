using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    [Authorize]
    public class ClassController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
