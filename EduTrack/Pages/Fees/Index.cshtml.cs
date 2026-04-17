using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.Fees
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IFeesService _feesService;

        public IndexModel(IFeesService feesService)
        {
            _feesService = feesService;
        }

        public List<FeesViewModel> Items { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            var fees = _feesService.GetAll();
            Items = fees.Select(f => new FeesViewModel
            {
                Fees_Id = f.Fees_Id,
                Class_Id = f.Class_Id,
                FeeType = (EduTrack.ViewModels.FeeType)(int)f.FeeType,
                Amount = f.Amount,
                Currency = f.Currency,
                Description = f.Description,
                IsActive = f.IsActive,
                IsDeleted = f.IsDeleted,
                Created_By = f.Created_By,
                Created_Date = f.Created_Date,
                Modified_By = f.Modified_By,
                Modified_Date = f.Modified_Date
            }).ToList();
        }
    }
}
