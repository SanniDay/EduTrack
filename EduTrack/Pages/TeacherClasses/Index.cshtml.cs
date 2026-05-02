using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.TeacherClasses
{
    [Authorize(Roles = "Admin,Teacher")]
    public class IndexModel : PageModel
    {
        private readonly ITeacherClassService _teacherClassService;
        private readonly ITeacherService _teacherService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;
        private readonly IClassSubjectService _classSubjectService;

        public IndexModel(ITeacherClassService teacherClassService, ITeacherService teacherService, IClassService classService, ISubjectService subjectService, IClassSubjectService classSubjectService)
        {
            _teacherClassService = teacherClassService;
            _teacherService = teacherService;
            _classService = classService;
            _subjectService = subjectService;
            _classSubjectService = classSubjectService;
        }

        public List<TeacherClassViewModel> TeacherClasses { get; set; } = new();

            [TempData]
            public string Message { get; set; }

            public void OnGet()
            {
                var teacherClasses = _teacherClassService.GetAllTeacherClasses();
                var teachers = _teacherService.GetAll();
                var classSubjects = _classSubjectService.GetAllClassSubjects();
                var classes = _classService.GetAllClasses();
                var subjects = _subjectService.GetAll();

                TeacherClasses = teacherClasses.Select(tc => new TeacherClassViewModel
                {
                    Teacher_Class_Id = tc.Teacher_Class_Id,
                    Teacher_Id = tc.Teacher_Id,
                    Class_Id = tc.Class_Id,
                    ClassSubject_Id = tc.ClassSubject_Id ?? 0,
                    IsActive = tc.isActive,
                    IsDeleted = tc.isDeleted,
                    Created_By = tc.Created_By,
                    Created_Date = tc.Created_Date,
                    Modified_By = tc.Modified_By,
                    Modified_Date = tc.Modified_Date,
                    TeacherName = teachers.FirstOrDefault(t => t.Teacher_Id == tc.Teacher_Id)?.FullName ?? "N/A",
                    ClassName = classes.FirstOrDefault(c => c.Class_Id == classSubjects.FirstOrDefault(cs => cs.ClassSubject_Id == tc.ClassSubject_Id)?.Class_Id)?.ClassName ?? "N/A",
                    Section = classes.FirstOrDefault(c => c.Class_Id == classSubjects.FirstOrDefault(cs => cs.ClassSubject_Id == tc.ClassSubject_Id)?.Class_Id)?.Section ?? "N/A",
                    SubjectName = subjects.FirstOrDefault(s => s.Subject_Id == classSubjects.FirstOrDefault(cs => cs.ClassSubject_Id == tc.ClassSubject_Id)?.Subject_Id)?.Subject_Name ?? "N/A"
                }).ToList();
            }
    }
}
