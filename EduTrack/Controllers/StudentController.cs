using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EduTrack.Constants;

namespace EduTrack.Controllers
{
    [Authorize(Roles = "" + AppRoles.Admin + "," + AppRoles.Teacher + "," + AppRoles.Student + "")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly ITeacherService _teacherService;
        private readonly IStudentClassService _studentClassService;
        private readonly ITeacherClassService _teacherClassService;
        private string Role => User.FindFirstValue(ClaimTypes.Role) ?? "";

        public StudentController(
            IStudentService studentService,
            IUserService userService,
            ITeacherService teacherService,
            IStudentClassService studentClassService,
            ITeacherClassService teacherClassService)
        {
            _studentService = studentService;
            _userService = userService;
            _teacherService = teacherService;
            _studentClassService = studentClassService;
            _teacherClassService = teacherClassService;
        }

        public IActionResult Index()
        {
            List<Student> students;

            if (User.IsInRole("Admin"))
            {
                students = _studentService.GetAll();
            }
            else if (User.IsInRole("Teacher"))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    var teacher = _teacherService.GetAll().FirstOrDefault(t => t.User_Id == userId);
                    if (teacher != null)
                    {
                        var classIds = _teacherClassService.GetAllTeacherClasses()
                            .Where(tc => tc.Teacher_Id == teacher.Teacher_Id)
                            .Select(tc => tc.Class_Id)
                            .ToHashSet();

                        var studentIds = _studentClassService.GetAllStudentClasses()
                            .Where(sc => classIds.Contains(sc.Class_Id))
                            .Select(sc => sc.Student_Id)
                            .ToHashSet();

                        students = _studentService.GetAll().Where(s => studentIds.Contains(s.Student_Id)).ToList();
                    }
                    else
                    {
                        students = new List<Student>();
                    }
                }
                else
                {
                    students = new List<Student>();
                }
            }
            else // Student
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    var student = _studentService.GetAll().FirstOrDefault(s => s.User_Id == userId);
                    students = student != null ? new List<Student> { student } : new List<Student>();
                }
                else
                {
                    students = new List<Student>();
                }
            }

            var model = students.Select(s => new StudentViewModel(
                s.Student_Id,
                s.User_Id,
                s.FullName ?? string.Empty,
                s.DOB,
                (ViewModels.Gender)(int)s.Gender,
                s.Phone_No ?? string.Empty,
                s.Address ?? string.Empty,
                s.Created_By ?? string.Empty,
                s.Created_Date,
                s.Modified_By ?? string.Empty,
                s.Modified_Date,
                s.IsActive,
                s.IsDeleted
            )).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new StudentViewModel(0, 0, string.Empty, DateTime.UtcNow, ViewModels.Gender.Male, string.Empty, string.Empty, "", DateTime.UtcNow, "", DateTime.UtcNow, true, false);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var student = new Student
            {
                User_Id = model.User_Id,
                FullName = model.FullName,
                DOB = model.DOB,
                Gender = (Models.Gender)model.Gender,
                Phone_No = model.Phone_No,
                Address = model.Address,
                Created_By = HttpContext.User.Identity?.Name ?? "System",
                Created_Date = DateTime.UtcNow,
                Modified_By = HttpContext.User.Identity?.Name ?? "System",
                Modified_Date = DateTime.UtcNow,
                IsActive = model.IsActive,
                IsDeleted = false
            };

            _studentService.Insert(student);
            TempData["Success"] = "Student created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var s = _studentService.GetById(id);
            if (s == null) return NotFound();

            var vm = new StudentViewModel(
                s.Student_Id,
                s.User_Id,
                s.FullName ?? string.Empty,
                s.DOB,
                (ViewModels.Gender)(int)s.Gender,
                s.Phone_No ?? string.Empty,
                s.Address ?? string.Empty,
                s.Created_By ?? string.Empty,
                s.Created_Date,
                s.Modified_By ?? string.Empty,
                s.Modified_Date,
                s.IsActive,
                s.IsDeleted
            );

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = _studentService.GetById(model.Student_Id);
            if (existing == null) return NotFound();

            existing.FullName = model.FullName;
            existing.DOB = model.DOB;
            existing.Gender = (Models.Gender)model.Gender;
            existing.Modified_By = HttpContext.User.Identity?.Name ?? "System";
            existing.Modified_Date = DateTime.UtcNow;
            existing.IsActive = model.IsActive;

            _studentService.Update(existing);

            var userAccount = _userService.GetById(model.User_Id);
            if (userAccount != null)
            {
                userAccount.PhoneNumber = model.Phone_No;
                userAccount.Address = model.Address;
                userAccount.IsActive = model.IsActive;
                userAccount.Modified_By = existing.Modified_By;
                _userService.Update(userAccount);
            }
            TempData["Success"] = "Student updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var s = _studentService.GetById(id);
            if (s == null) return NotFound();

            var vm = new StudentViewModel(
                s.Student_Id,
                s.User_Id,
                s.FullName ?? string.Empty,
                s.DOB,
                (ViewModels.Gender)(int)s.Gender,
                s.Phone_No ?? string.Empty,
                s.Address ?? string.Empty,
                s.Created_By ?? string.Empty,
                s.Created_Date,
                s.Modified_By ?? string.Empty,
                s.Modified_Date,
                s.IsActive,
                s.IsDeleted
            );

            return View(vm);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int Student_Id)
        {
            var s = _studentService.GetById(Student_Id);
            if (s == null) return NotFound();

            _studentService.Delete(Student_Id);
            TempData["Success"] = "Student deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
