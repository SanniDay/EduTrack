using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Pages.TimeTable
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    public class DetailsModel : PageModel
    {
        private readonly ITimeTableService _timeTableService;
        private readonly IClassService _classService;
        private readonly IClassSubjectService _classSubjectService;
        private readonly ITeacherService _teacherService;

        public DetailsModel(ITimeTableService timeTableService, IClassService classService, IClassSubjectService classSubjectService, ITeacherService teacherService)
        {
            _timeTableService = timeTableService;
            _classService = classService;
            _classSubjectService = classSubjectService;
            _teacherService = teacherService;
        }

        public TimeTableViewModel Item { get; set; }

        public IActionResult OnGet(int id)
        {
            var t = _timeTableService.GetById(id);
            if (t == null) return NotFound();

            Item = new TimeTableViewModel
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
                ClassName = _classService.GetAllClasses().FirstOrDefault(c => c.Class_Id == t.Class_Id)?.ClassName ?? "N/A",
                SubjectName = t.ClassSubject_Id.HasValue ? _classSubjectService.GetAllClassSubjects().FirstOrDefault(cs => cs.ClassSubject_Id == t.ClassSubject_Id.Value)?.Subject_Id.ToString() ?? "N/A" : "N/A",
                TeacherName = t.Teacher_Id.HasValue ? _teacherService.GetAll().FirstOrDefault(te => te.Teacher_Id == t.Teacher_Id.Value)?.FullName ?? "N/A" : "N/A"
            };

            return Page();
        }
    }
}
