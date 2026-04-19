using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.StudentClasses
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IStudentClassService _studentClassService;
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;

        public CreateModel(IStudentClassService studentClassService, IStudentService studentService, IClassService classService)
        {
            _studentClassService = studentClassService;
            _studentService = studentService;
            _classService = classService;
        }

        [BindProperty]
        public StudentClassViewModel StudentClassViewModel { get; set; }

        public List<Student> Students { get; set; } = new();
        public List<Class> Classes { get; set; } = new();

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            Students = _studentService.GetAll();
            Classes = _classService.GetAllClasses();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Students = _studentService.GetAll();
                Classes = _classService.GetAllClasses();
                return Page();
            }

            try
            {
                var studentClass = new StudentClass
                {
                    Student_Id = StudentClassViewModel.Student_Id,
                    Class_Id = StudentClassViewModel.Class_Id,
                    Created_By = User.Identity?.Name ?? "System",
                    Created_Date = DateTime.UtcNow,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow,
                    isActive = StudentClassViewModel.IsActive,
                    isDeleted = false
                };

                _studentClassService.CreateStudentClass(studentClass);

                TempData["Message"] = "Student added to class successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                Students = _studentService.GetAll();
                Classes = _classService.GetAllClasses();
                return Page();
            }
        }
    }
}
