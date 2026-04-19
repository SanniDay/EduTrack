using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.StudentFees
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IStudentFeesService _studentFeesService;

        public IndexModel(IStudentFeesService studentFeesService)
        {
            _studentFeesService = studentFeesService;
        }

        public List<StudentFeesViewModel> Items { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            var items = _studentFeesService.GetAll();
            Items = items.Select(s => new StudentFeesViewModel
            {
                StudentFees_Id = s.StudentFees_Id,
                Student_Id = s.Student_Id,
                Fees_Id = s.Fees_Id,
                Amount = s.Amount,
                Status = (EduTrack.ViewModels.PaymentStatus)(int)s.Status,
                DueDate = s.DueDate,
                PaidDate = s.PaidDate,
                PaymentMethod = s.PaymentMethod,
                Receipt_No = s.Receipt_No,
                Notes = s.Notes,
                IsDeleted = s.IsDeleted,
                Created_By = s.Created_By,
                Created_Date = s.Created_Date,
                Modified_By = s.Modified_By,
                Modified_Date = s.Modified_Date
            }).ToList();
        }
    }
}
