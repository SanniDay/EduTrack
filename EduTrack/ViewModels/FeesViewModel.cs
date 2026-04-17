using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public enum FeeType
    {
        Tuition = 1,
        Transport = 2,
        Lab = 3,
        Library = 4,
        Other = 5
    }

    public class FeesViewModel
    {
        public int Fees_Id { get; set; }

        [Required(ErrorMessage = "Class is required")]
        public int Class_Id { get; set; }

        [Required(ErrorMessage = "Fee type is required")]
        public FeeType FeeType { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Amount must be between 0.01 and 999,999.99")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [StringLength(3, ErrorMessage = "Currency code cannot exceed 3 characters")]
        public string Currency { get; set; } = "USD";

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; } = DateTime.UtcNow;

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public string ClassName { get; set; }
    }
}
