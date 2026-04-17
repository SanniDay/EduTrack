using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.Classes
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IClassService _classService;

        public EditModel(IClassService classService)
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

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var cls = new Class
                {
                    Class_Id = Class.Class_Id,
                    ClassName = Class.ClassName,
                    Section = Class.Section,
                    isActive = Class.IsActive,
                    isDeleted = Class.IsDeleted,
                    Created_By = Class.Created_By,
                    Created_Date = Class.Created_Date,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow
                };

                _classService.UpdateClass(cls);

                TempData["Message"] = "Class updated successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating class: {ex.Message}");
                return Page();
            }
        }
    }
}
