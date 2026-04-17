using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduTrack.Pages.Fees
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IFeesService _feesService;
        private readonly IClassService _classService;

        public EditModel(IFeesService feesService, IClassService classService)
        {
            _feesService = feesService;
            _classService = classService;
        }

        [BindProperty]
        public FeesViewModel Fee { get; set; } = new();

        public List<SelectListItem> Classes { get; set; } = new();

        private void LoadClasses()
        {
            var classes = _classService.GetAllClasses();
            Classes = classes.Select(c => new SelectListItem
            {
                Value = c.Class_Id.ToString(),
                Text = c.ClassName
            }).ToList();
        }

        public IActionResult OnGet(int id)
        {
            var fee = _feesService.GetById(id);
            if (fee == null)
            {
                return NotFound();
            }

            Fee = new FeesViewModel
            {
                Fees_Id = fee.Fees_Id,
                Class_Id = fee.Class_Id,
                FeeType = (EduTrack.ViewModels.FeeType)(int)fee.FeeType,
                Amount = fee.Amount,
                Currency = fee.Currency,
                Description = fee.Description,
                IsActive = fee.IsActive,
                IsDeleted = fee.IsDeleted,
                Created_By = fee.Created_By,
                Created_Date = fee.Created_Date,
                Modified_By = fee.Modified_By,
                Modified_Date = fee.Modified_Date
            };

            LoadClasses();
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                LoadClasses();
                return Page();
            }

            try
            {
                var fee = new EduTrack.Models.Fees
                {
                    Fees_Id = id,
                    Class_Id = Fee.Class_Id,
                    FeeType = (EduTrack.Models.FeeType)(int)Fee.FeeType,
                    Amount = Fee.Amount,
                    Currency = Fee.Currency,
                    Description = Fee.Description ?? string.Empty,
                    IsActive = Fee.IsActive,
                    IsDeleted = Fee.IsDeleted,
                    Created_By = Fee.Created_By,
                    Created_Date = Fee.Created_Date,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow
                };

                _feesService.Update(fee);

                TempData["Message"] = "Fee updated successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating fee: {ex.Message}");
                return Page();
            }
        }
    }
}
