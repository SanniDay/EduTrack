using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.ClassSubjects
{
    [Authorize(Roles = "Admin,Teacher")]
    public class IndexModel : PageModel
    {
        private readonly IClassSubjectService _classSubjectService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;

        public IndexModel(IClassSubjectService classSubjectService, IClassService classService, ISubjectService subjectService)
        {
            _classSubjectService = classSubjectService;
            _classService = classService;
            _subjectService = subjectService;
        }

        public List<ClassSubjectViewModel> ClassSubjects { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            var classSubjects = _classSubjectService.GetAllClassSubjects();
            var classes = _classService.GetAllClasses();
            var subjects = _subjectService.GetAll();

            ClassSubjects = classSubjects.Select(cs => new ClassSubjectViewModel
            {
                ClassSubject_Id = cs.ClassSubject_Id,
                Class_Id = cs.Class_Id,
                Subject_Id = cs.Subject_Id,
                IsCore = cs.IsCore,
                IsActive = cs.IsActive,
                IsDeleted = cs.IsDeleted,
                Created_By = cs.Created_By,
                Created_Date = cs.Created_Date,
                Modified_By = cs.Modified_By,
                Modified_Date = cs.Modified_Date,
                ClassName = classes.FirstOrDefault(c => c.Class_Id == cs.Class_Id)?.ClassName ?? "N/A",
                SubjectName = subjects.FirstOrDefault(s => s.Subject_Id == cs.Subject_Id)?.Subject_Name ?? "N/A"
            }).ToList();
        }
    }
}
