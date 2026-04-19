using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.Fees
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IFeesService _feesService;

        public DeleteModel(IFeesService feesService)
        {
            _feesService = feesService;
        }

        [BindProperty]
        public FeesViewModel Fee { get; set; } = new();

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

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                _feesService.Delete(id);
                TempData["Message"] = "Fee deleted successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error deleting fee: {ex.Message}");
                return Page();
            }
        }
    }
}
