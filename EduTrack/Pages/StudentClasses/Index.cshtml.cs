using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.StudentClasses
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IStudentClassService _studentClassService;
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;

        public IndexModel(IStudentClassService studentClassService, IStudentService studentService, IClassService classService)
        {
            _studentClassService = studentClassService;
            _studentService = studentService;
            _classService = classService;
        }

        public List<StudentClassViewModel> StudentClasses { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            var studentClasses = _studentClassService.GetAllStudentClasses();
            var students = _studentService.GetAll();
            var classes = _classService.GetAllClasses();

            StudentClasses = studentClasses.Select(sc => new StudentClassViewModel
            {
                Student_Class_Id = sc.Student_Class_Id,
                Student_Id = sc.Student_Id,
                Class_Id = sc.Class_Id,
                IsActive = sc.isActive,
                IsDeleted = sc.isDeleted,
                Created_By = sc.Created_By,
                Created_Date = sc.Created_Date,
                Modified_By = sc.Modified_By,
                Modified_Date = sc.Modified_Date,
                StudentName = students.FirstOrDefault(s => s.Student_Id == sc.Student_Id)?.FullName ?? "N/A",
                ClassName = classes.FirstOrDefault(c => c.Class_Id == sc.Class_Id)?.ClassName ?? "N/A",
                Section = classes.FirstOrDefault(c => c.Class_Id == sc.Class_Id)?.Section ?? "N/A"
            }).ToList();
        }
    }
}
