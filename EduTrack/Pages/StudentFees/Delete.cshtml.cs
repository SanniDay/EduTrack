using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.StudentFees
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IStudentFeesService _studentFeesService;

        public DeleteModel(IStudentFeesService studentFeesService)
        {
            _studentFeesService = studentFeesService;
        }

        [BindProperty]
        public StudentFeesViewModel Item { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var sf = _studentFeesService.GetById(id);
            if (sf == null)
            {
                return NotFound();
            }

            Item = new StudentFeesViewModel
            {
                StudentFees_Id = sf.StudentFees_Id,
                Student_Id = sf.Student_Id,
                Fees_Id = sf.Fees_Id,
                Amount = sf.Amount,
                Status = (EduTrack.ViewModels.PaymentStatus)(int)sf.Status,
                DueDate = sf.DueDate,
                PaidDate = sf.PaidDate,
                PaymentMethod = sf.PaymentMethod,
                Receipt_No = sf.Receipt_No,
                Notes = sf.Notes,
                IsDeleted = sf.IsDeleted,
                Created_By = sf.Created_By,
                Created_Date = sf.Created_Date,
                Modified_By = sf.Modified_By,
                Modified_Date = sf.Modified_Date
            };

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                _studentFeesService.Delete(id);
                TempData["Message"] = "Student fee record deleted successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error deleting record: {ex.Message}");
                return Page();
            }
        }
    }
}
