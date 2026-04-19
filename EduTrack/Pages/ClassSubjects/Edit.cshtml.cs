using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.ClassSubjects
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IClassSubjectService _classSubjectService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;

        public EditModel(IClassSubjectService classSubjectService, IClassService classService, ISubjectService subjectService)
        {
            _classSubjectService = classSubjectService;
            _classService = classService;
            _subjectService = subjectService;
        }

        [BindProperty]
        public ClassSubjectViewModel ClassSubjectViewModel { get; set; }

        public List<Class> Classes { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();

        public string ErrorMessage { get; set; }

        public IActionResult OnGet(int id)
        {
            var classSubject = _classSubjectService.GetClassSubjectById(id);

            if (classSubject == null)
                return NotFound();

            ClassSubjectViewModel = new ClassSubjectViewModel
            {
                ClassSubject_Id = classSubject.ClassSubject_Id,
                Class_Id = classSubject.Class_Id,
                Subject_Id = classSubject.Subject_Id,
                IsCore = classSubject.IsCore,
                IsActive = classSubject.IsActive,
                IsDeleted = classSubject.IsDeleted,
                Created_By = classSubject.Created_By,
                Created_Date = classSubject.Created_Date,
                Modified_By = classSubject.Modified_By,
                Modified_Date = classSubject.Modified_Date
            };

            Classes = _classService.GetAllClasses();
            Subjects = _subjectService.GetAll();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Classes = _classService.GetAllClasses();
                Subjects = _subjectService.GetAll();
                return Page();
            }

            try
            {
                var classSubject = new ClassSubject
                {
                    ClassSubject_Id = ClassSubjectViewModel.ClassSubject_Id,
                    Class_Id = ClassSubjectViewModel.Class_Id,
                    Subject_Id = ClassSubjectViewModel.Subject_Id,
                    IsCore = ClassSubjectViewModel.IsCore,
                    IsActive = ClassSubjectViewModel.IsActive,
                    IsDeleted = ClassSubjectViewModel.IsDeleted,
                    Created_By = ClassSubjectViewModel.Created_By,
                    Created_Date = ClassSubjectViewModel.Created_Date,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow
                };

                _classSubjectService.UpdateClassSubject(classSubject);

                TempData["Message"] = "Class Subject updated successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                Classes = _classService.GetAllClasses();
                Subjects = _subjectService.GetAll();
                return Page();
            }
        }
    }
}
