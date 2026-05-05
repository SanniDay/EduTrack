using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace EduTrack.Pages.Attendance
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    public class IndexModel : PageModel
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IStudentClassService _studentClassService;
        private readonly IStudentService _studentService;
        private readonly IClassSubjectService _classSubjectService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;
        private readonly ITeacherService _teacherService;
        private readonly ITeacherClassService _teacherClassService;

        public IndexModel(
            IAttendanceService attendanceService,
            IStudentClassService studentClassService,
            IStudentService studentService,
            IClassSubjectService classSubjectService,
            IClassService classService,
            ISubjectService subjectService,
            ITeacherService teacherService,
            ITeacherClassService teacherClassService)
        {
            _attendanceService = attendanceService;
            _studentClassService = studentClassService;
            _studentService = studentService;
            _classSubjectService = classSubjectService;
            _classService = classService;
            _subjectService = subjectService;
            _teacherService = teacherService;
            _teacherClassService = teacherClassService;
        }

        public List<AttendanceViewModel> Attendance { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            var attendance = _attendanceService.GetAllAttendance();

            if (User.IsInRole("Teacher"))
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    var teacher = _teacherService.GetAll().FirstOrDefault(t => t.User_Id == userId);
                    if (teacher != null)
                    {
                        var classIds = _teacherClassService.GetAllTeacherClasses()
                            .Where(tc => tc.Teacher_Id == teacher.Teacher_Id)
                            .Select(tc => tc.Class_Id)
                            .ToHashSet();

                        var studentClassIds = _studentClassService.GetAllStudentClasses()
                            .Where(sc => classIds.Contains(sc.Class_Id))
                            .Select(sc => sc.Student_Class_Id)
                            .ToHashSet();

                        attendance = attendance.Where(a => studentClassIds.Contains(a.Student_Class_Id)).ToList();
                    }
                    else
                    {
                        attendance = new List<Models.Attendance>();
                    }
                }
                else
                {
                    attendance = new List<Models.Attendance>();
                }
            }
            else if (User.IsInRole("Student"))
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    var student = _studentService.GetAll().FirstOrDefault(s => s.User_Id == userId);
                    if (student != null)
                    {
                        var studentClassIds = _studentClassService.GetAllStudentClasses()
                            .Where(sc => sc.Student_Id == student.Student_Id)
                            .Select(sc => sc.Student_Class_Id)
                            .ToHashSet();

                        attendance = attendance.Where(a => studentClassIds.Contains(a.Student_Class_Id)).ToList();
                    }
                    else
                    {
                        attendance = new List<Models.Attendance>();
                    }
                }
                else
                {
                    attendance = new List<Models.Attendance>();
                }
            }
            var studentClasses = _studentClassService.GetAllStudentClasses();
            var students = _studentService.GetAll();
            var classSubjects = _classSubjectService.GetAllClassSubjects();
            var classes = _classService.GetAllClasses();
            var subjects = _subjectService.GetAll();
            var teachers = _teacherService.GetAll();

            Attendance = attendance.Select(a => new AttendanceViewModel
            {
                Attendance_Id = a.Attendance_Id,
                Student_Class_Id = a.Student_Class_Id,
                ClassSubject_Id = a.ClassSubject_Id ?? 0,
                Attendance_Date = a.Attendance_Date,
                Status = a.Status,
                Marked_By_Teacher_Id = a.Marked_By_Teacher_Id,
                IsActive = a.IsActive,
                IsDeleted = a.IsDeleted,
                Created_By = a.Created_By,
                Created_Date = a.Created_Date,
                Modified_By = a.Modified_By,
                Modified_Date = a.Modified_Date,
                StudentName = students.FirstOrDefault(s => s.Student_Id == studentClasses.FirstOrDefault(sc => sc.Student_Class_Id == a.Student_Class_Id)?.Student_Id)?.FullName ?? "N/A",
                ClassName = classes.FirstOrDefault(c => c.Class_Id == classSubjects.FirstOrDefault(cs => cs.ClassSubject_Id == a.ClassSubject_Id)?.Class_Id)?.ClassName ?? "N/A",
                SubjectName = subjects.FirstOrDefault(s => s.Subject_Id == classSubjects.FirstOrDefault(cs => cs.ClassSubject_Id == a.ClassSubject_Id)?.Subject_Id)?.Subject_Name ?? "N/A",
                TeacherName = a.Marked_By_Teacher_Id.HasValue ? teachers.FirstOrDefault(t => t.Teacher_Id == a.Marked_By_Teacher_Id)?.FullName ?? "N/A" : "N/A"
            }).ToList();
        }
    }
}
