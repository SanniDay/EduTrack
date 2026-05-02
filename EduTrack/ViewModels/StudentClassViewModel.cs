using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public class StudentClassViewModel
    {
        public int Student_Class_Id { get; set; }

        [Required(ErrorMessage = "Student is required")]
        public int Student_Id { get; set; }

        [Required(ErrorMessage = "Class is required")]
        public int Class_Id { get; set; }

        [Required(ErrorMessage = "Enrollment date is required")]
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

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
        public string? Section { get; set; }
    }
}
