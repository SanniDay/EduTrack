namespace EduTrack.Models
{
    public enum FeeType
    {
        Tuition = 1,
        Transport = 2,
        Lab = 3,
        Library = 4,
        Other = 5
    }

    public class Fees
    {
        public int Fees_Id { get; set; }

        public int Class_Id { get; set; }

        public FeeType FeeType { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "USD";

        public string Description { get; set; } = string.Empty;

        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; }

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public Fees() { }

        public Fees(
            int fees_Id,
            int class_Id,
            FeeType feeType,
            decimal amount,
            string currency,
            string description,
            string created_By,
            DateTime created_Date,
            string modified_By,
            DateTime modified_Date,
            bool isActive,
            bool isDeleted)
        {
            Fees_Id = fees_Id;
            Class_Id = class_Id;
            FeeType = feeType;
            Amount = amount;
            Currency = currency;
            Description = description;
            Created_By = created_By;
            Created_Date = created_Date;
            Modified_By = modified_By;
            Modified_Date = modified_Date;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}
