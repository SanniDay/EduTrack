using System.Diagnostics;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EduTrack.Controllers
{
    [Authorize] //Require login for entire controller
    public class HomeController : Controller
    {
        // Only logged-in users can access
        public IActionResult Index()
        {
            return View();
        }

        // If you want Privacy public, allow anonymous
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        // Error page must be accessible without login
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        // Status code handler (404, 403 etc.)
        [AllowAnonymous]
        public IActionResult StatusCodeError(int code)
        {
            ViewBag.StatusCode = code;
            return View("StatusCodeError");
        }
    }
}