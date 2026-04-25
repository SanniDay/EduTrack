using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EduTrack.Constants;

namespace EduTrack.Controllers
{
    [Authorize(Roles = "" + AppRoles.Admin + "," + AppRoles.Teacher + "")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private string Role => User.FindFirstValue(ClaimTypes.Role) ?? "";

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            var students = _studentService.GetAll();

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
            existing.Phone_No = model.Phone_No;
            existing.Address = model.Address;
            existing.Modified_By = HttpContext.User.Identity?.Name ?? "System";
            existing.Modified_Date = DateTime.UtcNow;
            existing.IsActive = model.IsActive;

            _studentService.Update(existing);
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
