using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels

{
    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3
    }
    public class StudentViewModel

    {
        [Required(ErrorMessage = "Student Id is required")]
        public int Student_Id { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public int User_Id { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone_No { get; set; }


        public string Address { get; set; }


        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; } = DateTime.UtcNow;


        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public StudentViewModel
            (
            int student_Id,
            int user_Id,
            string fullName,
            DateTime dOB,
            Gender gender, 
            string phone_No,
            string address,
            string created_By,
            DateTime created_Date,
            string modified_By, 
            DateTime modified_Date,
            bool isActive,
            bool isDeleted
            )
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
