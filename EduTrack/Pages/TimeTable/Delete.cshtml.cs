using EduTrack.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Pages.TimeTable
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ITimeTableService _timeTableService;

        public DeleteModel(ITimeTableService timeTableService)
        {
            _timeTableService = timeTableService;
        }

        public int Id { get; set; }

        public IActionResult OnGet(int id)
        {
            var t = _timeTableService.GetById(id);
            if (t == null) return NotFound();
            Id = id;
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var res = _timeTableService.Delete(id);
            if (res <= 0)
            {
                TempData["Message"] = "Unable to delete timetable entry.";
                return RedirectToPage("Index");
            }

            TempData["Message"] = "TimeTable deleted (soft) successfully.";
            return RedirectToPage("Index");
        }
    }
}
