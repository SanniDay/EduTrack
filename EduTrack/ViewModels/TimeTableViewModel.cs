using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public class TimeTableViewModel
    {
        public int TimeTable_Id { get; set; }

        [Required(ErrorMessage = "Class is required")]
        [Display(Name = "Class")]
        public int Class_Id { get; set; }

        [Display(Name = "Class Subject")]
        public int? ClassSubject_Id { get; set; }

        [Display(Name = "Teacher")]
        public int? Teacher_Id { get; set; }

        [Required(ErrorMessage = "Day is required")]
        [Display(Name = "Day")]
        public string Day_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Period number is required")]
        [Range(1, 20, ErrorMessage = "Period must be between 1 and 20")]
        [Display(Name = "Period No")]
        public int Period_No { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public TimeSpan Start_Time { get; set; }

        [Required(ErrorMessage = "End time is required")]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public TimeSpan End_Time { get; set; }

        [StringLength(50)]
        [Display(Name = "Room No")]
        public string Room_No { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        // Display helpers
        public string ClassName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;

        public TimeTableViewModel() { }
    }
}
