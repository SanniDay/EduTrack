using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.Fees
{
    [Authorize(Roles = "Admin,Student,Teacher")]
    public class IndexModel : PageModel
    {
        private readonly IFeesService _feesService;
        private readonly IStudentService _studentService;
        private readonly IStudentClassService _studentClassService;
        private readonly ITeacherService _teacherService;
        private readonly ITeacherClassService _teacherClassService;

        public IndexModel(IFeesService feesService, IStudentService studentService, IStudentClassService studentClassService, ITeacherService teacherService, ITeacherClassService teacherClassService)
        {
            _feesService = feesService;
            _studentService = studentService;
            _studentClassService = studentClassService;
            _teacherService = teacherService;
            _teacherClassService = teacherClassService;
        }

        public List<FeesViewModel> Items { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            var fees = _feesService.GetAll();

            if (User.IsInRole("Student"))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    var student = _studentService.GetAll().FirstOrDefault(s => s.User_Id == userId);
                    if (student != null)
                    {
                        var studentClass = _studentClassService.GetAllStudentClasses().FirstOrDefault(sc => sc.Student_Id == student.Student_Id);
                        if (studentClass != null)
                        {
                            fees = fees.Where(f => f.Class_Id == studentClass.Class_Id).ToList();
                        }
                        else
                        {
                            fees = new List<Models.Fees>();
                        }
                    }
                    else
                    {
                        fees = new List<Models.Fees>();
                    }
                }
                else
                {
                    fees = new List<Models.Fees>();
                }
            }
            else if (User.IsInRole("Teacher"))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    var teacher = _teacherService.GetAll().FirstOrDefault(t => t.User_Id == userId);
                    if (teacher != null)
                    {
                        var classIds = _teacherClassService.GetAllTeacherClasses()
                            .Where(tc => tc.Teacher_Id == teacher.Teacher_Id)
                            .Select(tc => tc.Class_Id)
                            .ToHashSet();

                        fees = fees.Where(f => classIds.Contains(f.Class_Id)).ToList();
                    }
                    else
                    {
                        fees = new List<Models.Fees>();
                    }
                }
                else
                {
                    fees = new List<Models.Fees>();
                }
            }

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
