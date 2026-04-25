using EduTrack.Interfaces;
using EduTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.ClassSubjects
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IClassSubjectService _classSubjectService;

        public DeleteModel(IClassSubjectService classSubjectService)
        {
            _classSubjectService = classSubjectService;
        }

        [BindProperty]
        public ClassSubject ClassSubject { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            ClassSubject = _classSubjectService.GetClassSubjectById(id);

            if (ClassSubject == null)
                return NotFound();

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            _classSubjectService.DeleteClassSubject(id);

            TempData["Message"] = "Class Subject deleted successfully!";
            return RedirectToPage("Index");
        }
    }
}
