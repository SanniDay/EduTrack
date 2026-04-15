using System.Security.Claims;
using EduTrack.Constants;
using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITeacherService _teacherService;
        private readonly IRoleService _roleService;
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;

        public HomeController(
            IUserService userService, 
            ITeacherService teacherService, 
            IRoleService roleService,
            IStudentService studentService,
            IClassService classService)
        {
            _userService = userService;
            _teacherService = teacherService;
            _roleService = roleService;
            _studentService = studentService;
            _classService = classService;
        }

        public IActionResult Index()
        {
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "No Role";
            var name = User.Identity?.Name ?? "Unknown User";

            var model = new DashboardViewModel
            {
                UserRole = role,
                DisplayName = name
            };

            if (role == AppRoles.Admin)
            {
                // Load all admin metrics
                model.TotalUsers = _userService.GetAll().Count;
                model.TotalTeachers = _teacherService.GetAll().Count;
                model.TotalStudents = _studentService.GetAll().Count;
                model.TotalRoles = _roleService.GetAll().Count;
                model.TotalClasses = _classService.GetAllClasses().Count;
            }
            else if (role == AppRoles.Teacher)
            {
                // In future, restrict to this specific teacher ID
                model.TotalStudents = _studentService.GetAll().Count; 
                model.ActiveClasses = _classService.GetAllClasses().Count;
            }
            else if (role == AppRoles.Student)
            {
                // For a student, just passing enrolled classes count
                model.ActiveClasses = _classService.GetAllClasses().Count;
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult StatusCodeError(int code)
        {
            ViewBag.StatusCode = code;
            return View("StatusCodeError");
        }
    }
}