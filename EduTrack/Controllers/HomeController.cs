using System.Security.Claims;
using EduTrack.Constants;
using System.Linq;
using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduTrack.Models;
using System.Collections.Generic;

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
            ITimeTableService timeTableService)
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
                // Load all admin metrics
                model.TotalUsers = _userService.GetAll().Count;
                model.TotalTeachers = _teacherService.GetAll().Count;
                model.TotalStudents = _studentService.GetAll().Count;
                model.TotalRoles = _roleService.GetAll().Count;
                model.TotalClasses = _classService.GetAllClasses().Count;
            }
            else if (role == AppRoles.Teacher)
            {
                // For teacher, show assigned classes and total subjects they teach
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out var userId))
                {
                    // find teacher record for this user
                    var teachers = _teacherService.GetAll();
                    var teacher = teachers.FirstOrDefault(t => t.User_Id == userId);
                    if (teacher != null)
                    {
                        var teacherClasses = _teacherClassService.GetAllTeacherClasses()
                            .Where(tc => tc.Teacher_Id == teacher.Teacher_Id && !tc.isDeleted).ToList();
                        model.ActiveClasses = teacherClasses.Count;

                        // subjects taught by teacher (distinct)
                        var subjectIds = teacherClasses.Where(tc => tc.ClassSubject_Id.HasValue)
                            .Select(tc => _classSubjectService.GetClassSubjectById(tc.ClassSubject_Id.Value))
                            .Where(cs => cs != null && !cs.IsDeleted)
                            .Select(cs => cs.Subject_Id)
                            .Distinct()
                            .ToList();

                        model.TotalSubjects = subjectIds.Count;
                    }
                    // TIMETABLE: load teacher schedule
                    var allTts = _timeTableService.GetAll().Where(tt => !tt.IsDeleted).ToList();
                    var teacherTts = allTts.Where(tt => tt.Teacher_Id == teacher?.Teacher_Id).ToList();
                    PopulateTimetableWidgets(model, teacherTts);
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
                        var studentClasses = _studentClassService.GetAllStudentClasses()
                            .Where(sc => sc.Student_Id == student.Student_Id && !sc.isDeleted).ToList();
                        model.ActiveClasses = studentClasses.Count;

                        // total subjects for student's classes
                        var classIds = studentClasses.Select(sc => sc.Class_Id).Distinct().ToList();
                        var classSubjects = _classSubjectService.GetAllClassSubjects()
                            .Where(cs => classIds.Contains(cs.Class_Id) && !cs.IsDeleted).ToList();
                        model.TotalSubjects = classSubjects.Select(cs => cs.Subject_Id).Distinct().Count();

                        model.TotalStudents = 1; // helpful metric for student (self)
                        // TIMETABLE: load student timetable by student's class ids
                        var classIdsInner = studentClasses.Select(sc => sc.Class_Id).Distinct().ToList();
                        var allTtsInner = _timeTableService.GetAll().Where(tt => !tt.IsDeleted).ToList();
                        var studentTtsInner = allTtsInner.Where(tt => classIdsInner.Contains(tt.Class_Id)).ToList();
                        PopulateTimetableWidgets(model, studentTtsInner);
                    }
                }
            }

            return View(model);
        }

        // helper to populate timetable widgets in dashboard view model
        private void PopulateTimetableWidgets(DashboardViewModel model, List<TimeTable> tts)
        {
            if (tts == null) return;

            var classes = _classService.GetAllClasses().ToDictionary(c => c.Class_Id, c => c.ClassName);
            var subjects = _subjectService.GetAll().ToDictionary(s => s.Subject_Id, s => s.Subject_Name);
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
                    SubjectName = tt.ClassSubject_Id.HasValue && subjects.ContainsKey(tt.ClassSubject_Id.Value) ? subjects[tt.ClassSubject_Id.Value] : (tt.ClassSubject_Id.HasValue ? tt.ClassSubject_Id.Value.ToString() : string.Empty),
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