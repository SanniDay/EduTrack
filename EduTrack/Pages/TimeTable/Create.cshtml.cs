using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.TimeTable
{
    [Authorize(Roles = "Admin,Teacher")]
    public class CreateModel : PageModel
    {
        private readonly ITimeTableService _timeTableService;
        private readonly IClassService _classService;
        private readonly IClassSubjectService _classSubjectService;
        private readonly ITeacherService _teacherService;

        public CreateModel(ITimeTableService timeTableService, IClassService classService, IClassSubjectService classSubjectService, ITeacherService teacherService)
        {
            _timeTableService = timeTableService;
            _classService = classService;
            _classSubjectService = classSubjectService;
            _teacherService = teacherService;
        }

        [BindProperty]
        public TimeTableViewModel TimeTable { get; set; }

        public List<Class> Classes { get; set; } = new();
        public List<ClassSubject> ClassSubjects { get; set; } = new();
        public Dictionary<int, string> ClassSubjectOptions { get; set; } = new();
        public List<Teacher> Teachers { get; set; } = new();

        public void OnGet()
        {
            Classes = _classService.GetAllClasses();
            ClassSubjects = _classSubjectService.GetAllClassSubjects();
            Teachers = _teacherService.GetAll();
            // build display options for class-subject
            var classes = Classes.ToDictionary(c => c.Class_Id, c => c.ClassName);
            var subjects = (_classSubjectService.GetAllClassSubjects().Select(cs => cs.Subject_Id)).ToList();
            // use existing Subject service to get names
            var subjectNames = new Dictionary<int, string>();
            // try to resolve via IClassSubjectService doesn't provide subject names; callers will render by mapping
            var subjList = new List<int>();
            foreach (var cs in ClassSubjects)
            {
                subjList.Add(cs.Subject_Id);
            }
            ClassSubjectOptions = new Dictionary<int, string>();
            foreach (var cs in ClassSubjects)
            {
                var className = classes.ContainsKey(cs.Class_Id) ? classes[cs.Class_Id] : "Class";
                var subjName = cs.Subject_Id.ToString();
                ClassSubjectOptions[cs.ClassSubject_Id] = $"{className} - {subjName}";
            }
            TimeTable = new TimeTableViewModel();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Classes = _classService.GetAllClasses();
                ClassSubjects = _classSubjectService.GetAllClassSubjects();
                Teachers = _teacherService.GetAll();
                return Page();
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the highlighted errors.";
                Classes = _classService.GetAllClasses();
                ClassSubjects = _classSubjectService.GetAllClassSubjects();
                Teachers = _teacherService.GetAll();
                return Page();
            }

            if (TimeTable.End_Time <= TimeTable.Start_Time)
            {
                TempData["Error"] = "End Time must be greater than Start Time.";
                Classes = _classService.GetAllClasses();
                ClassSubjects = _classSubjectService.GetAllClassSubjects();
                Teachers = _teacherService.GetAll();
                return Page();
            }

            try
            {
                var timeTableModel = new EduTrack.Models.TimeTable
                {
                    Class_Id = TimeTable.Class_Id,
                    ClassSubject_Id = TimeTable.ClassSubject_Id,
                    Teacher_Id = TimeTable.Teacher_Id,
                    Day_Name = TimeTable.Day_Name,
                    Period_No = TimeTable.Period_No,
                    Start_Time = TimeTable.Start_Time,
                    End_Time = TimeTable.End_Time,
                    Room_No = TimeTable.Room_No,
                    IsActive = TimeTable.IsActive,
                    IsDeleted = false,
                    Created_By = User.Identity?.Name ?? "System",
                    Created_Date = DateTime.UtcNow
                };

                var createdId = _timeTableService.Create(timeTableModel);
                if (createdId == 0)
                {
                    TempData["Error"] = "Duplicate timetable entry or unable to create record.";
                    Classes = _classService.GetAllClasses();
                    ClassSubjects = _classSubjectService.GetAllClassSubjects();
                    Teachers = _teacherService.GetAll();
                    return Page();
                }
                TempData["Message"] = "TimeTable entry created successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
                Classes = _classService.GetAllClasses();
                ClassSubjects = _classSubjectService.GetAllClassSubjects();
                Teachers = _teacherService.GetAll();
                return Page();
            }
            }

            if (TimeTable.End_Time <= TimeTable.Start_Time)
            {
                ModelState.AddModelError(string.Empty, "End Time must be greater than Start Time.");
                Classes = _classService.GetAllClasses();
                ClassSubjects = _classSubjectService.GetAllClassSubjects();
                Teachers = _teacherService.GetAll();
                return Page();
            }

            var model = new EduTrack.Models.TimeTable
            {
                Class_Id = TimeTable.Class_Id,
                ClassSubject_Id = TimeTable.ClassSubject_Id,
                Teacher_Id = TimeTable.Teacher_Id,
                Day_Name = TimeTable.Day_Name,
                Period_No = TimeTable.Period_No,
                Start_Time = TimeTable.Start_Time,
                End_Time = TimeTable.End_Time,
                Room_No = TimeTable.Room_No,
                IsActive = TimeTable.IsActive,
                IsDeleted = false,
                Created_By = User.Identity?.Name ?? "System",
                Created_Date = DateTime.UtcNow
            };

            var newId = _timeTableService.Create(model);
            if (newId == 0)
            {
                ModelState.AddModelError(string.Empty, "Duplicate timetable entry or unable to create record.");
                Classes = _classService.GetAllClasses();
                ClassSubjects = _classSubjectService.GetAllClassSubjects();
                Teachers = _teacherService.GetAll();
                return Page();
            }

            TempData["Message"] = "TimeTable created successfully.";
            return RedirectToPage("Index");
        }
    }
}
