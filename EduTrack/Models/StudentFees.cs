namespace EduTrack.Models
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Overdue = 3,
        Exempted = 4,
        PartiallyPaid = 5
    }

    public class StudentFees
    {
        public int StudentFees_Id { get; set; }

        public int Student_Id { get; set; }

        public int Fees_Id { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? PaidDate { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string Receipt_No { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; }

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; }

        public bool IsDeleted { get; set; }

        public StudentFees() { }

        public StudentFees(
            int studentFees_Id,
            int student_Id,
            int fees_Id,
            DateTime dueDate,
            DateTime? paidDate,
            decimal amount,
            PaymentStatus status,
            string paymentMethod,
            string receipt_No,
            string notes,
            string created_By,
            DateTime created_Date,
            string modified_By,
            DateTime modified_Date,
            bool isDeleted)
        {
            StudentFees_Id = studentFees_Id;
            Student_Id = student_Id;
            Fees_Id = fees_Id;
            DueDate = dueDate;
            PaidDate = paidDate;
            Amount = amount;
            Status = status;
            PaymentMethod = paymentMethod;
            Receipt_No = receipt_No;
            Notes = notes;
            Created_By = created_By;
            Created_Date = created_Date;
            Modified_By = modified_By;
            Modified_Date = modified_Date;
            IsDeleted = isDeleted;
        }
    }
}
