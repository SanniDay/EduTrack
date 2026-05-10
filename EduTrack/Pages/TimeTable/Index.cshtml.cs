using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduTrack.Pages.TimeTable
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    public class IndexModel : PageModel
    {
        private readonly ITimeTableService _timeTableService;
        private readonly IClassService _classService;
        private readonly IStudentClassService _studentClassService;
        private readonly ITeacherService _teacherService;
        private readonly IStudentService _studentService;

        public IndexModel(ITimeTableService timeTableService, IClassService classService, IStudentClassService studentClassService, ITeacherService teacherService, IStudentService studentService)
        {
            _timeTableService = timeTableService;
            _classService = classService;
            _studentClassService = studentClassService;
            _teacherService = teacherService;
            _studentService = studentService;
        }

        public List<TimeTableViewModel> Items { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        public void OnGet(string search = "", int classId = 0, string day = "")
        {
            var tt = _timeTableService.GetAll().Where(t => !t.IsDeleted).ToList();

            if (User.IsInRole("Teacher"))
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out var userId))
                {
                    var teacher = _teacherService.GetAll().FirstOrDefault(t => t.User_Id == userId);
                    if (teacher != null)
                    {
                        tt = tt.Where(t => t.Teacher_Id == teacher.Teacher_Id).ToList();
                    }
                }
            }
            else if (User.IsInRole("Student"))
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out var userId))
                {
                    var student = _studentService.GetAll().FirstOrDefault(s => s.User_Id == userId);
                    if (student != null)
                    {
                        var enrolledClasses = _studentClassService.GetAllStudentClasses()
                            .Where(sc => sc.Student_Id == student.Student_Id && !sc.isDeleted)
                            .Select(sc => sc.Class_Id)
                            .ToList();
                        tt = tt.Where(t => enrolledClasses.Contains(t.Class_Id)).ToList();
                    }
                }
            }

            if (classId > 0)
                tt = tt.Where(t => t.Class_Id == classId).ToList();

            if (!string.IsNullOrWhiteSpace(day))
                tt = tt.Where(t => string.Equals(t.Day_Name, day, System.StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(search))
                tt = tt.Where(t => (t.Room_No ?? string.Empty).Contains(search, System.StringComparison.OrdinalIgnoreCase)).ToList();

            var classes = _classService.GetAllClasses().ToDictionary(c => c.Class_Id, c => c.ClassName);
            var studentClasses = _studentClassService.GetAllStudentClasses().ToDictionary(sc => sc.Student_Class_Id, sc => sc);

            Items = tt.Select(t => new TimeTableViewModel
            {
                TimeTable_Id = t.TimeTable_Id,
                Class_Id = t.Class_Id,
                ClassSubject_Id = t.ClassSubject_Id,
                Teacher_Id = t.Teacher_Id,
                Day_Name = t.Day_Name,
                Period_No = t.Period_No,
                Start_Time = t.Start_Time,
                End_Time = t.End_Time,
                Room_No = t.Room_No,
                IsActive = t.IsActive,
                IsDeleted = t.IsDeleted,
                ClassName = classes.ContainsKey(t.Class_Id) ? classes[t.Class_Id] : "N/A",
            }).ToList();
        }
    }
}
