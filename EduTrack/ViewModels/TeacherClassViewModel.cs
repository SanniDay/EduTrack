using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public class TeacherClassViewModel
    {
        public int Teacher_Class_Id { get; set; }

        [Required(ErrorMessage = "Teacher is required")]
        public int Teacher_Id { get; set; }

        [Required(ErrorMessage = "Class is required")]
        public int Class_Id { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public int Subject_Id { get; set; }

        [Required(ErrorMessage = "Assignment date is required")]
        public DateTime AssignmentDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; } = DateTime.UtcNow;

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public string TeacherName { get; set; }

        public string ClassName { get; set; }

        public string Section { get; set; }

        public string SubjectName { get; set; }

        public string SubjectCode { get; set; }
    }
}
