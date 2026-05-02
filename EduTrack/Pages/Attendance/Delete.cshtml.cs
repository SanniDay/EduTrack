using EduTrack.Interfaces;
using EduTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.Attendance
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IAttendanceService _attendanceService;

        public DeleteModel(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [BindProperty]
        public EduTrack.Models.Attendance AttendanceItem { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            AttendanceItem = _attendanceService.GetAttendanceById(id);

            if (AttendanceItem == null)
                return NotFound();

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            _attendanceService.DeleteAttendance(id);

            TempData["Message"] = "Attendance record deleted successfully!";
            return RedirectToPage("Index");
        }
    }
}
