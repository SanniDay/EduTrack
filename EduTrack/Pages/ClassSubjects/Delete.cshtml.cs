using EduTrack.Interfaces;
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

        public IActionResult OnGet(int id)
        {
            var classSubject = _classSubjectService.GetClassSubjectById(id);

            if (classSubject == null)
                return NotFound();

            _classSubjectService.DeleteClassSubject(id);

            TempData["Message"] = "Class Subject deleted successfully!";
            return RedirectToPage("Index");
        }
    }
}
