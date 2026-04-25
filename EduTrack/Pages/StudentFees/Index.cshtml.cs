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
        private readonly IStudentService _studentService;
        private readonly IFeesService _feesService;

        public IndexModel(IStudentFeesService studentFeesService, IStudentService studentService, IFeesService feesService)
        {
            _studentFeesService = studentFeesService;
            _studentService = studentService;
            _feesService = feesService;
        }

        public List<StudentFeesViewModel> Items { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            var items = _studentFeesService.GetAll();
            var students = _studentService.GetAll().ToDictionary(k => k.Student_Id, v => v.FullName);
            var fees = _feesService.GetAll().ToDictionary(k => k.Fees_Id, v => v.Description);

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
                Modified_Date = s.Modified_Date,
                StudentName = students.ContainsKey(s.Student_Id) ? students[s.Student_Id] : "N/A",
                FeeDescription = fees.ContainsKey(s.Fees_Id) ? fees[s.Fees_Id] : "N/A"
            }).ToList();
        }
    }
}
