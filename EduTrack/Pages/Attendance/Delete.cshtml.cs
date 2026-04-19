using EduTrack.Interfaces;
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

        public IActionResult OnGet(int id)
        {
            var attendance = _attendanceService.GetAttendanceById(id);

            if (attendance == null)
                return NotFound();

            _attendanceService.DeleteAttendance(id);

            TempData["Message"] = "Attendance record deleted successfully!";
            return RedirectToPage("Index");
        }
    }
}
