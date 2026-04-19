using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public class ClassViewModel
    {
        public int Class_Id { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        [StringLength(100, ErrorMessage = "Class name cannot exceed 100 characters")]
        public string ClassName { get; set; }

        [Required(ErrorMessage = "Section is required")]
        [StringLength(50, ErrorMessage = "Section cannot exceed 50 characters")]
        public string Section { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; } = DateTime.UtcNow;

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; } = DateTime.UtcNow;

        // Additional properties for display
        public int StudentCount { get; set; }

        public int TeacherCount { get; set; }
    }
}
