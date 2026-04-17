using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.TeacherClasses
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ITeacherClassService _teacherClassService;
        private readonly ITeacherService _teacherService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;

        public CreateModel(ITeacherClassService teacherClassService, ITeacherService teacherService, IClassService classService, ISubjectService subjectService)
        {
            _teacherClassService = teacherClassService;
            _teacherService = teacherService;
            _classService = classService;
            _subjectService = subjectService;
        }

        [BindProperty]
        public TeacherClassViewModel TeacherClassViewModel { get; set; }

        public List<Teacher> Teachers { get; set; } = new();
        public List<Class> Classes { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            Teachers = _teacherService.GetAll();
            Classes = _classService.GetAllClasses();
            Subjects = _subjectService.GetAll();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Teachers = _teacherService.GetAll();
                Classes = _classService.GetAllClasses();
                Subjects = _subjectService.GetAll();
                return Page();
            }

            try
            {
                var subject = _subjectService.GetById(TeacherClassViewModel.Subject_Id);
                var subjectName = subject?.Subject_Name ?? "Unknown";

                var teacherClass = new TeacherClass
                {
                    Teacher_Id = TeacherClassViewModel.Teacher_Id,
                    Class_Id = TeacherClassViewModel.Class_Id,
                    Subject = subjectName,
                    Created_By = User.Identity?.Name ?? "System",
                    Created_Date = DateTime.UtcNow,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow,
                    isActive = TeacherClassViewModel.IsActive,
                    isDeleted = false
                };

                _teacherClassService.CreateTeacherClass(teacherClass);

                TempData["Message"] = "Teacher assigned to class successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                Teachers = _teacherService.GetAll();
                Classes = _classService.GetAllClasses();
                Subjects = _subjectService.GetAll();
                return Page();
            }
        }
    }
}
