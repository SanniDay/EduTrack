using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            UserViewModel user = new UserViewModel();
            return View(user);
        }

        [HttpPost]
        public IActionResult Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Save to DB here
            return RedirectToAction("Index");
        }

    }
}
