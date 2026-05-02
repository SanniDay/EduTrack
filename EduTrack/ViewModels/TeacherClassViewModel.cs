using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public class TeacherClassViewModel
    {
        public int Teacher_Class_Id { get; set; }

        [Required(ErrorMessage = "Teacher is required")]
        public int Teacher_Id { get; set; }

        public int Class_Id { get; set; }

        [Required(ErrorMessage = "Class Subject is required")]
        public int ClassSubject_Id { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; } = DateTime.UtcNow;

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        public string? TeacherName { get; set; }

        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        public string? ClassName { get; set; }

        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        public string? Section { get; set; }

        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        public string? SubjectName { get; set; }
    }
}
