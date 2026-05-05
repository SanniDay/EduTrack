using System.Security.Claims;
using EduTrack.Constants;
using System.Linq;
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
        private readonly IStudentClassService _studentClassService;
        private readonly IClassSubjectService _classSubjectService;
        private readonly ISubjectService _subjectService;
        private readonly ITeacherClassService _teacherClassService;

        public HomeController(
            IUserService userService,
            ITeacherService teacherService,
            IRoleService roleService,
            IStudentService studentService,
            IClassService classService,
            IStudentClassService studentClassService,
            IClassSubjectService classSubjectService,
            ISubjectService subjectService,
            ITeacherClassService teacherClassService)
        {
            _userService = userService;
            _teacherService = teacherService;
            _roleService = roleService;
            _studentService = studentService;
            _classService = classService;
            _studentClassService = studentClassService;
            _classSubjectService = classSubjectService;
            _subjectService = subjectService;
            _teacherClassService = teacherClassService;
        }

        public IActionResult Index()
        {
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "No Role";
            var name = User.Identity?.Name ?? "Unknown User";

            var model = new DashboardViewModel
            {
                UserRole = role,
                DisplayName = name,
                CurrentDateTime = DateTime.Now.ToString("dddd, dd MMM yyyy HH:mm")
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
                // For teacher, show assigned classes and total subjects they teach
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out var userId))
                {
                    // find teacher record for this user
                    var teachers = _teacherService.GetAll();
                    var teacher = teachers.FirstOrDefault(t => t.User_Id == userId);
                    if (teacher != null)
                    {
                        var teacherClasses = _teacherClassService.GetAllTeacherClasses()
                            .Where(tc => tc.Teacher_Id == teacher.Teacher_Id && !tc.isDeleted).ToList();
                        model.ActiveClasses = teacherClasses.Count;

                        // subjects taught by teacher (distinct)
                        var subjectIds = teacherClasses.Where(tc => tc.ClassSubject_Id.HasValue)
                            .Select(tc => _classSubjectService.GetClassSubjectById(tc.ClassSubject_Id.Value))
                            .Where(cs => cs != null && !cs.IsDeleted)
                            .Select(cs => cs.Subject_Id)
                            .Distinct()
                            .ToList();

                        model.TotalSubjects = subjectIds.Count;
                    }
                }
            }
            else if (role == AppRoles.Student)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out var userId))
                {
                    var students = _studentService.GetAll();
                    var student = students.FirstOrDefault(s => s.User_Id == userId);
                    if (student != null)
                    {
                        var studentClasses = _studentClassService.GetAllStudentClasses()
                            .Where(sc => sc.Student_Id == student.Student_Id && !sc.isDeleted).ToList();
                        model.ActiveClasses = studentClasses.Count;

                        // total subjects for student's classes
                        var classIds = studentClasses.Select(sc => sc.Class_Id).Distinct().ToList();
                        var classSubjects = _classSubjectService.GetAllClassSubjects()
                            .Where(cs => classIds.Contains(cs.Class_Id) && !cs.IsDeleted).ToList();
                        model.TotalSubjects = classSubjects.Select(cs => cs.Subject_Id).Distinct().Count();

                        model.TotalStudents = 1; // helpful metric for student (self)
                    }
                }
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