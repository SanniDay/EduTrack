using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public class SubjectViewModel
    {
        public int Subject_Id { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        [StringLength(100, ErrorMessage = "Subject name cannot exceed 100 characters")]
        public string Subject_Name { get; set; }

        [Required(ErrorMessage = "Subject code is required")]
        [StringLength(20, ErrorMessage = "Subject code cannot exceed 20 characters")]
        public string Subject_Code { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; } = DateTime.UtcNow;

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; } = DateTime.UtcNow;
    }
}
