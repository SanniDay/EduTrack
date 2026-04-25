using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public class AttendanceViewModel
    {
        public int Attendance_Id { get; set; }

        [Required(ErrorMessage = "Student Class is required")]
        public int Student_Class_Id { get; set; }

        [Required(ErrorMessage = "Class Subject is required")]
        public int ClassSubject_Id { get; set; }

        [Required(ErrorMessage = "Attendance Date is required")]
        public DateTime Attendance_Date { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        public int? Marked_By_Teacher_Id { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; } = DateTime.UtcNow;

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        public string? StudentName { get; set; }

        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        public string? ClassName { get; set; }

        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        public string? SubjectName { get; set; }

        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        public string? TeacherName { get; set; }
    }
}
