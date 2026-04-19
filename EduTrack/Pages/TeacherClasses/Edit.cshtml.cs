using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.TeacherClasses
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ITeacherClassService _teacherClassService;
        private readonly ITeacherService _teacherService;
        private readonly IClassSubjectService _classSubjectService;

        public EditModel(ITeacherClassService teacherClassService, ITeacherService teacherService, IClassSubjectService classSubjectService)
        {
            _teacherClassService = teacherClassService;
            _teacherService = teacherService;
            _classSubjectService = classSubjectService;
        }

        [BindProperty]
        public TeacherClassViewModel TeacherClassViewModel { get; set; }

        public List<Teacher> Teachers { get; set; } = new();
        public List<ClassSubject> ClassSubjects { get; set; } = new();

        public string ErrorMessage { get; set; }

        public IActionResult OnGet(int id)
        {
            var teacherClass = _teacherClassService.GetTeacherClassById(id);
            if (teacherClass == null)
            {
                return NotFound();
            }

            TeacherClassViewModel = new TeacherClassViewModel
            {
                Teacher_Class_Id = teacherClass.Teacher_Class_Id,
                Teacher_Id = teacherClass.Teacher_Id,
                Class_Id = teacherClass.Class_Id,
                ClassSubject_Id = teacherClass.ClassSubject_Id ?? 0,
                IsActive = teacherClass.isActive,
                IsDeleted = teacherClass.isDeleted,
                Created_By = teacherClass.Created_By,
                Created_Date = teacherClass.Created_Date,
                Modified_By = teacherClass.Modified_By,
                Modified_Date = teacherClass.Modified_Date
            };

            Teachers = _teacherService.GetAll();
            ClassSubjects = _classSubjectService.GetAllClassSubjects();

            return Page();
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
                    Teacher_Class_Id = TeacherClassViewModel.Teacher_Class_Id,
                    Teacher_Id = TeacherClassViewModel.Teacher_Id,
                    ClassSubject_Id = TeacherClassViewModel.ClassSubject_Id,
                    Created_By = TeacherClassViewModel.Created_By,
                    Created_Date = TeacherClassViewModel.Created_Date,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow,
                    isActive = TeacherClassViewModel.IsActive,
                    isDeleted = TeacherClassViewModel.IsDeleted
                };

                _teacherClassService.UpdateTeacherClass(teacherClass);

                TempData["Message"] = "Teacher class updated successfully!";
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
