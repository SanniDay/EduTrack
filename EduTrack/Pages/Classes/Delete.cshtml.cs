using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.Classes
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IClassService _classService;

        public DeleteModel(IClassService classService)
        {
            _classService = classService;
        }

        [BindProperty]
        public ClassViewModel Class { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var cls = _classService.GetClassById(id);
            if (cls == null)
            {
                return NotFound();
            }

            Class = new ClassViewModel
            {
                Class_Id = cls.Class_Id,
                ClassName = cls.ClassName,
                Section = cls.Section,
                IsActive = cls.isActive,
                IsDeleted = cls.isDeleted,
                Created_By = cls.Created_By,
                Created_Date = cls.Created_Date,
                Modified_By = cls.Modified_By,
                Modified_Date = cls.Modified_Date
            };

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                _classService.DeleteClass(id);
                TempData["Message"] = "Class deleted successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error deleting class: {ex.Message}");
                return Page();
            }
        }
    }
}
