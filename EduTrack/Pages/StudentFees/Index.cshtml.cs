using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.StudentFees
{
    [Authorize(Roles = "Admin,Student,Teacher")]
    public class IndexModel : PageModel
    {
        private readonly IStudentFeesService _studentFeesService;
        private readonly IStudentService _studentService;
        private readonly IFeesService _feesService;
        private readonly IStudentClassService _studentClassService;
        private readonly ITeacherService _teacherService;
        private readonly ITeacherClassService _teacherClassService;

        public IndexModel(
            IStudentFeesService studentFeesService,
            IStudentService studentService,
            IFeesService feesService,
            IStudentClassService studentClassService,
            ITeacherService teacherService,
            ITeacherClassService teacherClassService)
        {
            _studentFeesService = studentFeesService;
            _studentService = studentService;
            _feesService = feesService;
            _studentClassService = studentClassService;
            _teacherService = teacherService;
            _teacherClassService = teacherClassService;
        }

        public List<StudentFeesViewModel> Items { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            List<EduTrack.Models.StudentFees> items;

            if (User.IsInRole("Student"))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    var student = _studentService.GetAll().FirstOrDefault(s => s.User_Id == userId);
                    items = student != null ? _studentFeesService.GetByStudentId(student.Student_Id) : new List<EduTrack.Models.StudentFees>();
                }
                else
                {
                    items = new List<EduTrack.Models.StudentFees>();
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

                        var studentIds = _studentClassService.GetAllStudentClasses()
                            .Where(sc => classIds.Contains(sc.Class_Id))
                            .Select(sc => sc.Student_Id)
                            .ToHashSet();

                        var list = new List<EduTrack.Models.StudentFees>();
                        foreach (var sid in studentIds)
                        {
                            var sf = _studentFeesService.GetByStudentId(sid);
                            if (sf != null && sf.Any()) list.AddRange(sf);
                        }

                        items = list;
                    }
                    else
                    {
                        items = new List<EduTrack.Models.StudentFees>();
                    }
                }
                else
                {
                    items = new List<EduTrack.Models.StudentFees>();
                }
            }
            else
            {
                items = _studentFeesService.GetAll();
            }

            var students = _studentService.GetAll().ToDictionary(k => k.Student_Id, v => v.FullName);
            var fees = _feesService.GetAll().ToDictionary(k => k.Fees_Id, v => v.Description);

            Items = items.Select(s => new StudentFeesViewModel
            {
                StudentFees_Id = s.StudentFees_Id,
                Student_Id = s.Student_Id,
                Fees_Id = s.Fees_Id,
                Amount = s.Amount,
                Status = (EduTrack.ViewModels.PaymentStatus)(int)s.Status,
                DueDate = s.DueDate,
                PaidDate = s.PaidDate,
                PaymentMethod = s.PaymentMethod,
                Receipt_No = s.Receipt_No,
                Notes = s.Notes,
                IsDeleted = s.IsDeleted,
                Created_By = s.Created_By,
                Created_Date = s.Created_Date,
                Modified_By = s.Modified_By,
                Modified_Date = s.Modified_Date,
                StudentName = students.ContainsKey(s.Student_Id) ? students[s.Student_Id] : "N/A",
                FeeDescription = fees.ContainsKey(s.Fees_Id) ? fees[s.Fees_Id] : "N/A"
            }).ToList();
        }
    }
}
