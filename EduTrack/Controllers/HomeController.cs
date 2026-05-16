using System.Security.Claims;
using EduTrack.Constants;
using System.Linq;
using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduTrack.Models;
using System.Collections.Generic;
using FeePaymentStatus = EduTrack.Models.PaymentStatus;

namespace EduTrack.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITeacherService _teacherService;
        private readonly IRoleService _roleService;
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;
        private readonly IStudentClassService _studentClassService;
        private readonly IClassSubjectService _classSubjectService;
        private readonly ISubjectService _subjectService;
        private readonly ITeacherClassService _teacherClassService;
        private readonly ITimeTableService _timeTableService;
        private readonly IStudentFeesService _studentFeesService;
        private readonly IFeesService _feesService;
        private readonly IAttendanceService _attendanceService;

        public HomeController(
            IUserService userService,
            ITeacherService teacherService,
            IRoleService roleService,
            IStudentService studentService,
            IClassService classService,
            IStudentClassService studentClassService,
            IClassSubjectService classSubjectService,
            ISubjectService subjectService,
            ITeacherClassService teacherClassService,
            ITimeTableService timeTableService,
            IStudentFeesService studentFeesService,
            IFeesService feesService,
            IAttendanceService attendanceService)
        {
            _userService = userService;
            _teacherService = teacherService;
            _roleService = roleService;
            _studentService = studentService;
            _classService = classService;
            _studentClassService = studentClassService;
            _classSubjectService = classSubjectService;
            _subjectService = subjectService;
            _teacherClassService = teacherClassService;
            _timeTableService = timeTableService;
            _studentFeesService = studentFeesService;
            _feesService = feesService;
            _attendanceService = attendanceService;
        }

        public IActionResult Index()
        {
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "No Role";
            var name = User.Identity?.Name ?? "Unknown User";

            var model = new DashboardViewModel
            {
                UserRole = role,
                DisplayName = name,
                CurrentDateTime = DateTime.Now.ToString("dddd, dd MMM yyyy HH:mm")
            };

            if (role == AppRoles.Admin)
            {
                var users = _userService.GetAll();
                var teachers = _teacherService.GetAll();
                var students = _studentService.GetAll();
                var classes = _classService.GetAllClasses();
                var classSubjects = _classSubjectService.GetAllClassSubjects();
                var fees = _feesService.GetAll();
                var studentFees = _studentFeesService.GetAll().Where(sf => !sf.IsDeleted).ToList();
                var todayAttendance = _attendanceService.GetAllAttendance()
                    .Where(a => !a.IsDeleted && a.Attendance_Date.Date == DateTime.Today)
                    .ToList();

                model.TotalUsers = users.Count;
                model.ActiveUsers = users.Count(u => u.IsActive && !u.IsDeleted);
                model.TotalTeachers = teachers.Count;
                model.ActiveTeachers = teachers.Count(t => t.IsActive && !t.IsDeleted);
                model.TotalStudents = students.Count;
                model.ActiveStudents = students.Count(s => s.IsActive && !s.IsDeleted);
                model.TotalRoles = _roleService.GetAll().Count;
                model.TotalClasses = classes.Count(c => !c.isDeleted);
                model.TotalSubjects = _subjectService.GetAll().Count(s => !s.IsDeleted);
                model.TotalClassSubjects = classSubjects.Count(cs => cs.IsActive && !cs.IsDeleted);
                model.TotalFeeStructures = fees.Count(f => f.IsActive && !f.IsDeleted);
                model.TimetableEntries = _timeTableService.GetAll().Count(tt => tt.IsActive && !tt.IsDeleted);
                model.PendingFeesCount = studentFees.Count(sf => sf.Status == FeePaymentStatus.Pending || sf.Status == FeePaymentStatus.PartiallyPaid);
                model.OverdueFeesCount = studentFees.Count(sf => sf.Status == FeePaymentStatus.Overdue);
                model.CollectedFeesAmount = studentFees.Where(sf => sf.Status == FeePaymentStatus.Paid).Sum(sf => sf.Amount);
                model.PendingFeesAmount = studentFees
                    .Where(sf => sf.Status == FeePaymentStatus.Pending || sf.Status == FeePaymentStatus.PartiallyPaid || sf.Status == FeePaymentStatus.Overdue)
                    .Sum(sf => sf.Amount);
                model.TodayAttendanceMarked = todayAttendance.Count;
                model.PresentToday = todayAttendance.Count(a => IsPresentStatus(a.Status));
                model.AbsentToday = todayAttendance.Count(a => IsAbsentStatus(a.Status));
            }
            else if (role == AppRoles.Teacher)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out var userId))
                {
                    var teachers = _teacherService.GetAll();
                    var teacher = teachers.FirstOrDefault(t => t.User_Id == userId);
                    if (teacher != null)
                    {
                        model.TeacherName = teacher.FullName;

                        var teacherClasses = _teacherClassService.GetAllTeacherClasses()
                            .Where(tc => tc.Teacher_Id == teacher.Teacher_Id && tc.isActive && !tc.isDeleted)
                            .ToList();
                        model.ActiveClasses = teacherClasses.Select(tc => tc.Class_Id).Distinct().Count();

                        var subjectIds = teacherClasses.Where(tc => tc.ClassSubject_Id != null)
                            .Select(tc => _classSubjectService.GetClassSubjectById(tc.ClassSubject_Id.GetValueOrDefault()))
                            .Where(cs => cs is { IsDeleted: false })
                            .Select(cs => cs!.Subject_Id)
                            .Distinct()
                            .ToList();

                        model.TotalSubjects = subjectIds.Count;

                        var classIds = teacherClasses.Select(tc => tc.Class_Id).Distinct().ToList();
                        model.EnrolledStudents = _studentClassService.GetAllStudentClasses()
                            .Count(sc => classIds.Contains(sc.Class_Id) && sc.isActive && !sc.isDeleted);

                        var allTts = _timeTableService.GetAll().Where(tt => tt.IsActive && !tt.IsDeleted).ToList();
                        var teacherTts = allTts.Where(tt => tt.Teacher_Id == teacher.Teacher_Id).ToList();
                        model.TimetableEntries = teacherTts.Count;
                        PopulateTimetableWidgets(model, teacherTts);
                        model.TodayClassCount = model.TodayClasses.Count;

                        model.AttendanceMarkedByMeToday = _attendanceService.GetAllAttendance()
                            .Count(a => !a.IsDeleted
                                && a.Attendance_Date.Date == DateTime.Today
                                && a.Marked_By_Teacher_Id == teacher.Teacher_Id);
                    }
                }
            }
            else if (role == AppRoles.Student)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out var userId))
                {
                    var students = _studentService.GetAll();
                    var student = students.FirstOrDefault(s => s.User_Id == userId);
                    if (student != null)
                    {
                        model.StudentName = student.FullName;

                        var studentClasses = _studentClassService.GetAllStudentClasses()
                            .Where(sc => sc.Student_Id == student.Student_Id && sc.isActive && !sc.isDeleted).ToList();
                        model.ActiveClasses = studentClasses.Count;

                        var classIds = studentClasses.Select(sc => sc.Class_Id).Distinct().ToList();
                        var classSubjects = _classSubjectService.GetAllClassSubjects()
                            .Where(cs => classIds.Contains(cs.Class_Id) && cs.IsActive && !cs.IsDeleted).ToList();
                        model.TotalSubjects = classSubjects.Select(cs => cs.Subject_Id).Distinct().Count();
                        model.PrimaryClassName = GetClassDisplayName(student.Class_Id);

                        model.TotalStudents = 1; // helpful metric for student (self)
                        var allTtsInner = _timeTableService.GetAll().Where(tt => tt.IsActive && !tt.IsDeleted).ToList();
                        var studentTtsInner = allTtsInner.Where(tt => classIds.Contains(tt.Class_Id)).ToList();
                        model.TimetableEntries = studentTtsInner.Count;
                        PopulateTimetableWidgets(model, studentTtsInner);
                        model.TodayClassCount = model.TodayClasses.Count;

                        var studentClassIds = studentClasses.Select(sc => sc.Student_Class_Id).ToList();
                        var attendance = _attendanceService.GetAllAttendance()
                            .Where(a => !a.IsDeleted && studentClassIds.Contains(a.Student_Class_Id))
                            .ToList();
                        model.AttendanceRecords = attendance.Count;
                        model.PresentRecords = attendance.Count(a => IsPresentStatus(a.Status));
                        model.AttendancePercentage = model.AttendanceRecords == 0
                            ? 0
                            : Math.Round((decimal)model.PresentRecords * 100 / model.AttendanceRecords, 1);

                        var myFees = _studentFeesService.GetByStudentId(student.Student_Id)
                            .Where(sf => !sf.IsDeleted)
                            .ToList();
                        model.PaidFeesCount = myFees.Count(sf => sf.Status == FeePaymentStatus.Paid);
                        model.PendingFeesCount = myFees.Count(sf => sf.Status == FeePaymentStatus.Pending
                            || sf.Status == FeePaymentStatus.PartiallyPaid
                            || sf.Status == FeePaymentStatus.Overdue);
                        model.OverdueFeesCount = myFees.Count(sf => sf.Status == FeePaymentStatus.Overdue);
                        model.MyPaidFeesAmount = myFees.Where(sf => sf.Status == FeePaymentStatus.Paid).Sum(sf => sf.Amount);
                        model.MyPendingFeesAmount = myFees
                            .Where(sf => sf.Status == FeePaymentStatus.Pending
                                || sf.Status == FeePaymentStatus.PartiallyPaid
                                || sf.Status == FeePaymentStatus.Overdue)
                            .Sum(sf => sf.Amount);
                    }
                }
            }

            return View(model);
        }

        // helper to populate timetable widgets in dashboard view model
        private void PopulateTimetableWidgets(DashboardViewModel model, List<TimeTable> tts)
        {
            if (tts == null) return;

            var classes = _classService.GetAllClasses().ToDictionary(
                c => c.Class_Id,
                c => string.IsNullOrWhiteSpace(c.Section) ? c.ClassName : $"{c.ClassName} - {c.Section}");
            var subjects = _subjectService.GetAll().ToDictionary(s => s.Subject_Id, s => s.Subject_Name);
            var classSubjects = _classSubjectService.GetAllClassSubjects().ToDictionary(cs => cs.ClassSubject_Id, cs => cs.Subject_Id);
            var teachers = _teacherService.GetAll().ToDictionary(t => t.Teacher_Id, t => t.FullName);

            // initialize week days
            var days = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            var weekly = new Dictionary<string, List<TimeTableViewModel>>();
            foreach (var d in days) weekly[d] = new List<TimeTableViewModel>();

            foreach (var tt in tts)
            {
                var vm = new TimeTableViewModel
                {
                    TimeTable_Id = tt.TimeTable_Id,
                    Class_Id = tt.Class_Id,
                    ClassSubject_Id = tt.ClassSubject_Id,
                    Teacher_Id = tt.Teacher_Id,
                    Day_Name = tt.Day_Name,
                    Period_No = tt.Period_No,
                    Start_Time = tt.Start_Time,
                    End_Time = tt.End_Time,
                    Room_No = tt.Room_No,
                    IsActive = tt.IsActive,
                    IsDeleted = tt.IsDeleted,
                    ClassName = classes.ContainsKey(tt.Class_Id) ? classes[tt.Class_Id] : "",
                    SubjectName = GetSubjectDisplayName(tt.ClassSubject_Id, classSubjects, subjects),
                    TeacherName = tt.Teacher_Id.HasValue && teachers.ContainsKey(tt.Teacher_Id.Value) ? teachers[tt.Teacher_Id.Value] : (tt.Teacher_Id.HasValue ? tt.Teacher_Id.Value.ToString() : string.Empty)
                };

                if (!string.IsNullOrWhiteSpace(vm.Day_Name) && weekly.ContainsKey(vm.Day_Name))
                {
                    weekly[vm.Day_Name].Add(vm);
                }
            }

            // sort each day by Start_Time or Period_No
            foreach (var k in weekly.Keys.ToList())
                weekly[k] = weekly[k].OrderBy(x => x.Start_Time).ThenBy(x => x.Period_No).ToList();

            model.WeeklyTimetable = weekly;

            // Today's classes
            var today = DateTime.Now.DayOfWeek.ToString();
            if (!weekly.ContainsKey(today))
            {
                // try to map Monday..Saturday when Sunday
                if (today == "Sunday") today = "Monday"; // fallback
            }

            if (weekly.ContainsKey(today))
            {
                var todayList = weekly[today];
                model.TodayClasses = todayList;

                var now = DateTime.Now.TimeOfDay;
                model.CurrentClass = todayList.FirstOrDefault(c => c.Start_Time <= now && c.End_Time > now);
                model.NextClass = todayList.Where(c => c.Start_Time > now).OrderBy(c => c.Start_Time).FirstOrDefault();
            }
        }

        private string GetClassDisplayName(int classId)
        {
            var classItem = _classService.GetAllClasses().FirstOrDefault(c => c.Class_Id == classId);
            if (classItem == null) return string.Empty;

            return string.IsNullOrWhiteSpace(classItem.Section)
                ? classItem.ClassName
                : $"{classItem.ClassName} - {classItem.Section}";
        }

        private static string GetSubjectDisplayName(
            int? classSubjectId,
            Dictionary<int, int> classSubjects,
            Dictionary<int, string> subjects)
        {
            if (!classSubjectId.HasValue) return string.Empty;
            if (!classSubjects.TryGetValue(classSubjectId.Value, out var subjectId)) return $"Subject #{classSubjectId.Value}";
            return subjects.TryGetValue(subjectId, out var subjectName) ? subjectName : $"Subject #{subjectId}";
        }

        private static bool IsPresentStatus(string? status)
        {
            return string.Equals(status, "Present", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsAbsentStatus(string? status)
        {
            return string.Equals(status, "Absent", StringComparison.OrdinalIgnoreCase);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult StatusCodeError(int code)
        {
            ViewBag.StatusCode = code;
            return View("StatusCodeError");
        }
    }
}
