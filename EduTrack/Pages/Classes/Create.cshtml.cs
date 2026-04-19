using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.Classes
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IClassService _classService;

        public CreateModel(IClassService classService)
        {
            _classService = classService;
        }

        [BindProperty]
        public ClassViewModel Class { get; set; } = new();

        public void OnGet()
        {
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
                    ClassName = Class.ClassName,
                    Section = Class.Section,
                    isActive = Class.IsActive,
                    isDeleted = Class.IsDeleted,
                    Created_By = User.Identity?.Name ?? "System",
                    Created_Date = DateTime.UtcNow,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow
                };

                _classService.CreateClass(cls);

                TempData["Message"] = "Class created successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating class: {ex.Message}");
                return Page();
            }
        }
    }
}
