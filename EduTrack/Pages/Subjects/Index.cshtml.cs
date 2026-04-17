using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.Subjects
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ISubjectService _subjectService;

        public IndexModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public List<SubjectViewModel> Subjects { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            var subjects = _subjectService.GetAll();
            Subjects = subjects.Select(s => new SubjectViewModel
            {
                Subject_Id = s.Subject_Id,
                Subject_Name = s.Subject_Name,
                Subject_Code = s.Subject_Code,
                Description = s.Description,
                IsActive = s.IsActive,
                IsDeleted = s.IsDeleted,
                Created_By = s.Created_By,
                Created_Date = s.Created_Date,
                Modified_By = s.Modified_By,
                Modified_Date = s.Modified_Date
            }).ToList();
        }
    }
}
