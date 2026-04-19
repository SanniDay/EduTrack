namespace EduTrack.Models
{
    public class Subject
    {
        public int Subject_Id { get; set; }

        public string Subject_Name { get; set; } = string.Empty;

        public string Subject_Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; }

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public Subject() { }

        public Subject(
            int subject_Id,
            string subject_Name,
            string subject_Code,
            string description,
            string created_By,
            DateTime created_Date,
            string modified_By,
            DateTime modified_Date,
            bool isActive,
            bool isDeleted)
        {
            Subject_Id = subject_Id;
            Subject_Name = subject_Name;
            Subject_Code = subject_Code;
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
