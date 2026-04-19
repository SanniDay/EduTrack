using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.ClassSubjects
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IClassSubjectService _classSubjectService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;

        public CreateModel(IClassSubjectService classSubjectService, IClassService classService, ISubjectService subjectService)
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

        public void OnGet()
        {
            Classes = _classService.GetAllClasses();
            Subjects = _subjectService.GetAll();
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
                    Class_Id = ClassSubjectViewModel.Class_Id,
                    Subject_Id = ClassSubjectViewModel.Subject_Id,
                    IsCore = ClassSubjectViewModel.IsCore,
                    IsActive = ClassSubjectViewModel.IsActive,
                    IsDeleted = false,
                    Created_By = User.Identity?.Name ?? "System",
                    Created_Date = DateTime.UtcNow,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow
                };

                _classSubjectService.CreateClassSubject(classSubject);

                TempData["Message"] = "Class Subject created successfully!";
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
