using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduTrack.Pages.TimeTable
{
    [Authorize(Roles = "Admin,Teacher")]
    public class EditModel : PageModel
    {
        private readonly ITimeTableService _timeTableService;
        private readonly IClassService _classService;
        private readonly IClassSubjectService _classSubjectService;
        private readonly ITeacherService _teacherService;

        public EditModel(ITimeTableService timeTableService, IClassService classService, IClassSubjectService classSubjectService, ITeacherService teacherService)
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
        public Dictionary<int,string> ClassSubjectOptions { get; set; } = new();
        public List<Teacher> Teachers { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var t = _timeTableService.GetById(id);
            if (t == null) return NotFound();

            TimeTable = new TimeTableViewModel
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
                IsActive = t.IsActive
            };

            Classes = _classService.GetAllClasses();
            ClassSubjects = _classSubjectService.GetAllClassSubjects();
            Teachers = _teacherService.GetAll();
            var classes = Classes.ToDictionary(c => c.Class_Id, c => c.ClassName);

            ClassSubjectOptions = new Dictionary<int, string>();

            foreach (var cs in ClassSubjects)
            {
                // Show only Subject Name
                ClassSubjectOptions[cs.ClassSubject_Id] = cs.SubjectName;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
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
                var model = new EduTrack.Models.TimeTable
                {
                    TimeTable_Id = TimeTable.TimeTable_Id,
                    Class_Id = TimeTable.Class_Id,
                    ClassSubject_Id = TimeTable.ClassSubject_Id,
                    Teacher_Id = TimeTable.Teacher_Id,
                    Day_Name = TimeTable.Day_Name,
                    Period_No = TimeTable.Period_No,
                    Start_Time = TimeTable.Start_Time,
                    End_Time = TimeTable.End_Time,
                    Room_No = TimeTable.Room_No,
                    IsActive = TimeTable.IsActive,
                    Modified_By = User.Identity?.Name ?? "System",
                    Modified_Date = DateTime.UtcNow
                };

                var res = _timeTableService.Update(model);
                if (res == 0)
                {
                    TempData["Error"] = "Duplicate timetable entry or unable to update record.";
                    Classes = _classService.GetAllClasses();
                    ClassSubjects = _classSubjectService.GetAllClassSubjects();
                    Teachers = _teacherService.GetAll();
                    return Page();
                }
                TempData["Message"] = "TimeTable entry updated successfully!";
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
    }
}
