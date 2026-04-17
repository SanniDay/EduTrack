using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.Subjects
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ISubjectService _subjectService;

        public DeleteModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [BindProperty]
        public SubjectViewModel Subject { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var subject = _subjectService.GetById(id);
            if (subject == null)
            {
                return NotFound();
            }

            Subject = new SubjectViewModel
            {
                Subject_Id = subject.Subject_Id,
                Subject_Name = subject.Subject_Name,
                Subject_Code = subject.Subject_Code,
                Description = subject.Description,
                IsActive = subject.IsActive,
                IsDeleted = subject.IsDeleted,
                Created_By = subject.Created_By,
                Created_Date = subject.Created_Date,
                Modified_By = subject.Modified_By,
                Modified_Date = subject.Modified_Date
            };

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                _subjectService.Delete(id);
                TempData["Message"] = "Subject deleted successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error deleting subject: {ex.Message}");
                return Page();
            }
        }
    }
}
