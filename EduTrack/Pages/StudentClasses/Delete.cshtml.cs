using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.StudentClasses
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IStudentClassService _studentClassService;
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;

        public DeleteModel(IStudentClassService studentClassService, IStudentService studentService, IClassService classService)
        {
            _studentClassService = studentClassService;
            _studentService = studentService;
            _classService = classService;
        }

        public StudentClassViewModel StudentClassViewModel { get; set; }

        public IActionResult OnGet(int id)
        {
            var studentClass = _studentClassService.GetStudentClassById(id);
            if (studentClass == null)
            {
                return NotFound();
            }

            var student = _studentService.GetById(studentClass.Student_Id);
            var cls = _classService.GetClassById(studentClass.Class_Id);

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
                Modified_Date = studentClass.Modified_Date,
                StudentName = student != null ? student.FullName : "N/A",
                ClassName = cls?.ClassName ?? "N/A",
                Section = cls?.Section ?? "N/A"
            };

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                _studentClassService.DeleteStudentClass(id);
                TempData["Message"] = "Student class deleted successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting student class: {ex.Message}";
                return RedirectToPage("Index");
            }
        }
    }
}
