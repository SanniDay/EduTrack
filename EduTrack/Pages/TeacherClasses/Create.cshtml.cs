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
        private readonly IClassSubjectService _classSubjectService;

        public CreateModel(ITeacherClassService teacherClassService, ITeacherService teacherService, IClassService classService, IClassSubjectService classSubjectService)
        {
            _teacherClassService = teacherClassService;
            _teacherService = teacherService;
            _classService = classService;
            _classSubjectService = classSubjectService;
        }

        [BindProperty]
        public TeacherClassViewModel TeacherClassViewModel { get; set; }

        public List<Teacher> Teachers { get; set; } = new();
        public List<ClassSubject> ClassSubjects { get; set; } = new();

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            Teachers = _teacherService.GetAll();
            ClassSubjects = _classSubjectService.GetAllClassSubjects();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Teachers = _teacherService.GetAll();
                ClassSubjects = _classSubjectService.GetAllClassSubjects();
                return Page();
            }

            try
            {
                var teacherClass = new TeacherClass
                {
                    Teacher_Id = TeacherClassViewModel.Teacher_Id,
                    ClassSubject_Id = TeacherClassViewModel.ClassSubject_Id,
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
                ClassSubjects = _classSubjectService.GetAllClassSubjects();
                return Page();
            }
        }
    }
}
