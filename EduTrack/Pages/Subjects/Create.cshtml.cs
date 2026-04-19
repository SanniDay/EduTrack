using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.Subjects
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ISubjectService _subjectService;

        public CreateModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [BindProperty]
        public SubjectViewModel Subject { get; set; } = new();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var subject = new Subject
                {
                    Subject_Name = Subject.Subject_Name,
                    Subject_Code = Subject.Subject_Code,
                    Description = Subject.Description,
                    IsActive = Subject.IsActive,
                    IsDeleted = Subject.IsDeleted,
                    Created_By = User.Identity?.Name ?? "System",
                    Created_Date = DateTime.UtcNow,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow
                };

                _subjectService.Create(subject);

                TempData["Message"] = "Subject created successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating subject: {ex.Message}");
                return Page();
            }
        }
    }
}
