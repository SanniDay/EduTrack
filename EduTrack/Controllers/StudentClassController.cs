using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    [Authorize]
    public class StudentClassController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
