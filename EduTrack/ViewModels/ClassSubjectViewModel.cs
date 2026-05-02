using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public class ClassSubjectViewModel
    {
        public int ClassSubject_Id { get; set; }

        [Required(ErrorMessage = "Class is required")]
        public int Class_Id { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public int Subject_Id { get; set; }

        public bool IsCore { get; set; } = true;

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; } = DateTime.UtcNow;

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        public string? ClassName { get; set; }

        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        public string? SubjectName { get; set; }
    }
}
