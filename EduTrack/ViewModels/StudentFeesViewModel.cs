using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Overdue = 3,
        Exempted = 4,
        PartiallyPaid = 5
    }

    public class StudentFeesViewModel
    {
        public int StudentFees_Id { get; set; }

        [Required(ErrorMessage = "Student is required")]
        public int Student_Id { get; set; }

        [Required(ErrorMessage = "Fee is required")]
        public int Fees_Id { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Amount must be between 0.01 and 999,999.99")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment status is required")]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [Required(ErrorMessage = "Due date is required")]
        public DateTime DueDate { get; set; }

        public DateTime? PaidDate { get; set; }

        [StringLength(50, ErrorMessage = "Payment method cannot exceed 50 characters")]
        public string PaymentMethod { get; set; }

        [StringLength(100, ErrorMessage = "Receipt number cannot exceed 100 characters")]
        public string Receipt_No { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string Notes { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; } = DateTime.UtcNow;

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public string StudentName { get; set; }
        public string FeeDescription { get; set; }
    }
}
