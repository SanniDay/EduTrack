using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AttendanceModel = EduTrack.Models.Attendance;
using System.Linq;

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
        private readonly ITeacherClassService _teacherClassService;

        public CreateModel(
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
            // If teacher, limit lists to their assigned classes and students
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

                        StudentClasses = _studentClassService.GetAllStudentClasses()
                            .Where(sc => classIds.Contains(sc.Class_Id)).ToList();

                        var studentIds = StudentClasses.Select(sc => sc.Student_Id).ToHashSet();

                        Students = _studentService.GetAll().Where(s => studentIds.Contains(s.Student_Id)).ToList();

                        ClassSubjects = _classSubjectService.GetAllClassSubjects().Where(cs => classIds.Contains(cs.Class_Id)).ToList();

                        Classes = _classService.GetAllClasses().Where(c => classIds.Contains(c.Class_Id)).ToList();

                        Subjects = _subjectService.GetAll().Where(s => ClassSubjects.Any(cs => cs.Subject_Id == s.Subject_Id)).ToList();

                        Teachers = new List<Teacher> { teacher };
                        return;
                    }
                }

                // fallback to empty lists
                StudentClasses = new List<StudentClass>();
                Students = new List<Student>();
                ClassSubjects = new List<ClassSubject>();
                Classes = new List<Class>();
                Subjects = new List<Subject>();
                Teachers = new List<Teacher>();
                return;
            }

            // Admin path
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
                // If teacher, validate student_class belongs to teacher's classes and set marked by teacher
                if (User.IsInRole("Teacher"))
                {
                    var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (int.TryParse(userIdClaim, out var userId))
                    {
                        var teacher = _teacherService.GetAll().FirstOrDefault(t => t.User_Id == userId);
                        if (teacher == null)
                        {
                            ModelState.AddModelError(string.Empty, "Teacher record not found.");
                            StudentClasses = _studentClassService.GetAllStudentClasses();
                            Students = _studentService.GetAll();
                            ClassSubjects = _classSubjectService.GetAllClassSubjects();
                            Classes = _classService.GetAllClasses();
                            Subjects = _subjectService.GetAll();
                            Teachers = _teacherService.GetAll();
                            return Page();
                        }

                        var sc = _studentClassService.GetStudentClassById(AttendanceViewModel.Student_Class_Id);
                        if (sc == null)
                        {
                            ModelState.AddModelError(string.Empty, "Invalid student selection.");
                            StudentClasses = _studentClassService.GetAllStudentClasses();
                            Students = _studentService.GetAll();
                            ClassSubjects = _classSubjectService.GetAllClassSubjects();
                        Classes = _classService.GetAllClasses();
                        Subjects = _subjectService.GetAll();
                            Teachers = _teacherService.GetAll();
                            return Page();
                        }

                        var teacherClassIds = _teacherClassService.GetAllTeacherClasses().Where(tc => tc.Teacher_Id == teacher.Teacher_Id).Select(tc => tc.Class_Id).ToHashSet();
                        if (!teacherClassIds.Contains(sc.Class_Id))
                        {
                            ModelState.AddModelError(string.Empty, "You are not authorized to mark attendance for this student/class.");
                            StudentClasses = _studentClassService.GetAllStudentClasses();
                            Students = _studentService.GetAll();
                            ClassSubjects = _classSubjectService.GetAllClassSubjects();
                            Classes = _classService.GetAllClasses();
                            Subjects = _subjectService.GetAll();
                            Teachers = _teacherService.GetAll();
                            return Page();
                        }

                        // ensure marked by teacher id is set to current teacher
                        AttendanceViewModel.Marked_By_Teacher_Id = teacher.Teacher_Id;
                    }
                }

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
