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
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;

        public EditModel(ITeacherClassService teacherClassService, ITeacherService teacherService, IClassService classService, ISubjectService subjectService)
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

        public IActionResult OnGet(int id)
        {
            var teacherClass = _teacherClassService.GetTeacherClassById(id);
            if (teacherClass == null)
            {
                return NotFound();
            }

            var subject = _subjectService.GetAll().FirstOrDefault(s => s.Subject_Name == teacherClass.Subject);

            TeacherClassViewModel = new TeacherClassViewModel
            {
                Teacher_Class_Id = teacherClass.Teacher_Class_Id,
                Teacher_Id = teacherClass.Teacher_Id,
                Class_Id = teacherClass.Class_Id,
                Subject_Id = subject?.Subject_Id ?? 0,
                IsActive = teacherClass.isActive,
                IsDeleted = teacherClass.isDeleted,
                Created_By = teacherClass.Created_By,
                Created_Date = teacherClass.Created_Date,
                Modified_By = teacherClass.Modified_By,
                Modified_Date = teacherClass.Modified_Date
            };

            Teachers = _teacherService.GetAll();
            Classes = _classService.GetAllClasses();
            Subjects = _subjectService.GetAll();

            return Page();
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
                    Teacher_Class_Id = TeacherClassViewModel.Teacher_Class_Id,
                    Teacher_Id = TeacherClassViewModel.Teacher_Id,
                    Class_Id = TeacherClassViewModel.Class_Id,
                    Subject = subjectName,
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
                Classes = _classService.GetAllClasses();
                Subjects = _subjectService.GetAll();
                return Page();
            }
        }
    }
}
