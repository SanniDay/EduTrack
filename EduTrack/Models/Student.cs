using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrack.Models
{

    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3
    }
    public class Student
    {
        public int Student_Id { get; set; }

        public int? User_Id { get; set; }

        [StringLength(100)]
        public string? FullName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [StringLength(10)]
        public Gender Gender { get; set; }

        [StringLength(15)]
        public string? Phone_No { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; } = string.Empty;

        public DateTime? Created_Date { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string Modified_By { get; set; } = string.Empty;

        public DateTime? Modified_Date { get; set; } = DateTime.UtcNow;

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public Student(int student_Id, int? user_Id, string? fullName, DateTime? dOB, Gender gender, string? phone_No, string? address, string created_By, DateTime? created_Date, string modified_By, DateTime? modified_Date, bool? isActive, bool? isDeleted)
        {
            Student_Id = student_Id;
            User_Id = user_Id;
            FullName = fullName;
            DOB = dOB;
            Gender = gender;
            Phone_No = phone_No;
            Address = address;
            Created_By = created_By;
            Created_Date = created_Date;
            Modified_By = modified_By;
            Modified_Date = modified_Date;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}
