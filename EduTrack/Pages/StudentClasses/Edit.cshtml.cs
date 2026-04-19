using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.StudentClasses
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IStudentClassService _studentClassService;
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;

        public EditModel(IStudentClassService studentClassService, IStudentService studentService, IClassService classService)
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

        public IActionResult OnGet(int id)
        {
            var studentClass = _studentClassService.GetStudentClassById(id);
            if (studentClass == null)
            {
                return NotFound();
            }

            StudentClassViewModel = new StudentClassViewModel
            {
                Student_Class_Id = studentClass.Student_Class_Id,
                Student_Id = studentClass.Student_Id,
                Class_Id = studentClass.Class_Id,
                IsActive = studentClass.isActive,
                IsDeleted = studentClass.isDeleted,
                Created_By = studentClass.Created_By,
                Created_Date = studentClass.Created_Date,
                Modified_By = studentClass.Modified_By,
                Modified_Date = studentClass.Modified_Date
            };

            Students = _studentService.GetAll();
            Classes = _classService.GetAllClasses();

            return Page();
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
                    Student_Class_Id = StudentClassViewModel.Student_Class_Id,
                    Student_Id = StudentClassViewModel.Student_Id,
                    Class_Id = StudentClassViewModel.Class_Id,
                    Created_By = StudentClassViewModel.Created_By,
                    Created_Date = StudentClassViewModel.Created_Date,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow,
                    isActive = StudentClassViewModel.IsActive,
                    isDeleted = StudentClassViewModel.IsDeleted
                };

                _studentClassService.UpdateStudentClass(studentClass);

                TempData["Message"] = "Student class updated successfully!";
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
