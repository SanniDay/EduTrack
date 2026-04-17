using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduTrack.Pages.StudentFees
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IStudentFeesService _studentFeesService;
        private readonly IStudentService _studentService;
        private readonly IFeesService _feesService;

        public EditModel(IStudentFeesService studentFeesService, IStudentService studentService, IFeesService feesService)
        {
            _studentFeesService = studentFeesService;
            _studentService = studentService;
            _feesService = feesService;
        }

        [BindProperty]
        public StudentFeesViewModel Item { get; set; } = new();

        public List<SelectListItem> Students { get; set; } = new();
        public List<SelectListItem> Fees { get; set; } = new();

        private void LoadStudents()
        {
            var students = _studentService.GetAll();
            Students = students.Select(s => new SelectListItem
            {
                Value = s.Student_Id.ToString(),
                Text = s.FullName
            }).ToList();
        }

        private void LoadFees()
        {
            var fees = _feesService.GetAll();
            Fees = fees.Select(f => new SelectListItem
            {
                Value = f.Fees_Id.ToString(),
                Text = $"{f.FeeType} - {f.Amount} {f.Currency}"
            }).ToList();
        }

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

            LoadStudents();
            LoadFees();
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                LoadStudents();
                LoadFees();
                return Page();
            }

            try
            {
                var model = new EduTrack.Models.StudentFees
                {
                    StudentFees_Id = id,
                    Student_Id = Item.Student_Id,
                    Fees_Id = Item.Fees_Id,
                    Amount = Item.Amount,
                    Status = (EduTrack.Models.PaymentStatus)(int)Item.Status,
                    DueDate = Item.DueDate,
                    PaidDate = Item.PaidDate,
                    PaymentMethod = Item.PaymentMethod ?? string.Empty,
                    Receipt_No = Item.Receipt_No ?? string.Empty,
                    Notes = Item.Notes ?? string.Empty,
                    IsDeleted = Item.IsDeleted,
                    Created_By = Item.Created_By,
                    Created_Date = Item.Created_Date,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow
                };

                _studentFeesService.Update(model);
                TempData["Message"] = "Student fee record updated successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating record: {ex.Message}");
                return Page();
            }
        }
    }
}
