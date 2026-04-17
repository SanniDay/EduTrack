using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.TeacherClasses
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ITeacherClassService _teacherClassService;
        private readonly ITeacherService _teacherService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;

        public DeleteModel(ITeacherClassService teacherClassService, ITeacherService teacherService, IClassService classService, ISubjectService subjectService)
        {
            _teacherClassService = teacherClassService;
            _teacherService = teacherService;
            _classService = classService;
            _subjectService = subjectService;
        }

        public TeacherClassViewModel TeacherClassViewModel { get; set; }

        public IActionResult OnGet(int id)
        {
            var teacherClass = _teacherClassService.GetTeacherClassById(id);
            if (teacherClass == null)
            {
                return NotFound();
            }

            var teacher = _teacherService.GetById(teacherClass.Teacher_Id);
            var cls = _classService.GetClassById(teacherClass.Class_Id);
            var subjects = _subjectService.GetAll();

            TeacherClassViewModel = new TeacherClassViewModel
            {
                Teacher_Class_Id = teacherClass.Teacher_Class_Id,
                Teacher_Id = teacherClass.Teacher_Id,
                Class_Id = teacherClass.Class_Id,
                IsActive = teacherClass.isActive,
                IsDeleted = teacherClass.isDeleted,
                Created_By = teacherClass.Created_By,
                Created_Date = teacherClass.Created_Date,
                Modified_By = teacherClass.Modified_By,
                Modified_Date = teacherClass.Modified_Date,
                TeacherName = teacher != null ? teacher.FullName : "N/A",
                ClassName = cls?.ClassName ?? "N/A",
                Section = cls?.Section ?? "N/A",
                SubjectName = teacherClass.Subject
            };

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                _teacherClassService.DeleteTeacherClass(id);
                TempData["Message"] = "Teacher class deleted successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting teacher class: {ex.Message}";
                return RedirectToPage("Index");
            }
        }
    }
}
