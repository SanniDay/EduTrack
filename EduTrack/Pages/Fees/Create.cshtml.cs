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
    public class CreateModel : PageModel
    {
        private readonly IFeesService _feesService;
        private readonly IClassService _classService;

        public CreateModel(IFeesService feesService, IClassService classService)
        {
            _feesService = feesService;
            _classService = classService;
        }

        [BindProperty]
        public FeesViewModel Fee { get; set; } = new();

        public List<SelectListItem> Classes { get; set; } = new();

        public void OnGet()
        {
            LoadClasses();
        }

        private void LoadClasses()
        {
            var classes = _classService.GetAllClasses();
            Classes = classes.Select(c => new SelectListItem
            {
                Value = c.Class_Id.ToString(),
                Text = c.ClassName
            }).ToList();
        }

        public IActionResult OnPost()
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
                    Class_Id = Fee.Class_Id,
                    FeeType = (EduTrack.Models.FeeType)Fee.FeeType,
                    Amount = Fee.Amount,
                    Currency = Fee.Currency,
                    Description = Fee.Description ?? string.Empty,
                    IsActive = Fee.IsActive,
                    IsDeleted = Fee.IsDeleted,
                    Created_By = User.Identity?.Name ?? "System",
                    Created_Date = DateTime.UtcNow,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow
                };

                _feesService.Create(fee);

                TempData["Message"] = "Fee created successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating fee: {ex.Message}");
                return Page();
            }
        }
    }
}
