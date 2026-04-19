using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.Classes
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IClassService _classService;

        public IndexModel(IClassService classService)
        {
            _classService = classService;
        }

        public List<ClassViewModel> Classes { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            var classes = _classService.GetAllClasses();
            Classes = classes.Select(c => new ClassViewModel
            {
                Class_Id = c.Class_Id,
                ClassName = c.ClassName,
                Section = c.Section,
                IsActive = c.isActive,
                IsDeleted = c.isDeleted,
                Created_By = c.Created_By,
                Created_Date = c.Created_Date,
                Modified_By = c.Modified_By,
                Modified_Date = c.Modified_Date
            }).ToList();
        }
    }
}
