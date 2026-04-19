using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AttendanceModel = EduTrack.Models.Attendance;

namespace EduTrack.Pages.Attendance
{
    [Authorize(Roles = "Admin,Teacher")]
    public class CreateModel : PageModel
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IStudentClassService _studentClassService;
        private readonly IStudentService _studentService;
        private readonly IClassSubjectService _classSubjectService;
        private readonly IClassService _classService;
        private readonly ISubjectService _subjectService;
        private readonly ITeacherService _teacherService;

        public CreateModel(
            IAttendanceService attendanceService,
            IStudentClassService studentClassService,
            IStudentService studentService,
            IClassSubjectService classSubjectService,
            IClassService classService,
            ISubjectService subjectService,
            ITeacherService teacherService)
        {
            _attendanceService = attendanceService;
            _studentClassService = studentClassService;
            _studentService = studentService;
            _classSubjectService = classSubjectService;
            _classService = classService;
            _subjectService = subjectService;
            _teacherService = teacherService;
        }

        [BindProperty]
        public AttendanceViewModel AttendanceViewModel { get; set; }

        public List<StudentClass> StudentClasses { get; set; } = new();
        public List<Student> Students { get; set; } = new();
        public List<ClassSubject> ClassSubjects { get; set; } = new();
        public List<Class> Classes { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();
        public List<Teacher> Teachers { get; set; } = new();

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            StudentClasses = _studentClassService.GetAllStudentClasses();
            Students = _studentService.GetAll();
            ClassSubjects = _classSubjectService.GetAllClassSubjects();
            Classes = _classService.GetAllClasses();
            Subjects = _subjectService.GetAll();
            Teachers = _teacherService.GetAll();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                StudentClasses = _studentClassService.GetAllStudentClasses();
                Students = _studentService.GetAll();
                ClassSubjects = _classSubjectService.GetAllClassSubjects();
                Classes = _classService.GetAllClasses();
                Subjects = _subjectService.GetAll();
                Teachers = _teacherService.GetAll();
                return Page();
            }

            try
            {
                var attendance = new AttendanceModel
                {
                    Student_Class_Id = AttendanceViewModel.Student_Class_Id,
                    ClassSubject_Id = AttendanceViewModel.ClassSubject_Id,
                    Attendance_Date = AttendanceViewModel.Attendance_Date,
                    Status = AttendanceViewModel.Status,
                    Marked_By_Teacher_Id = AttendanceViewModel.Marked_By_Teacher_Id,
                    IsActive = true,
                    IsDeleted = false,
                    Created_By = User.Identity?.Name ?? "System",
                    Created_Date = DateTime.UtcNow,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow
                };

                _attendanceService.CreateAttendance(attendance);

                TempData["Message"] = "Attendance recorded successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                StudentClasses = _studentClassService.GetAllStudentClasses();
                Students = _studentService.GetAll();
                ClassSubjects = _classSubjectService.GetAllClassSubjects();
                Classes = _classService.GetAllClasses();
                Subjects = _subjectService.GetAll();
                Teachers = _teacherService.GetAll();
                return Page();
            }
        }
    }
}
