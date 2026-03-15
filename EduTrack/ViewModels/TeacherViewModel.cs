using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public class TeacherViewModel
    {
        [Required(ErrorMessage = "Teacher Id is required")]
        public int Teacher_Id { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public int User_Id { get; set; }

        [Required(ErrorMessage = "Teacher name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone_No { get; set; }


        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; } = DateTime.UtcNow;

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }


        public TeacherViewModel(
            int teacher_Id,
            int user_Id,
            string fullName,
            string phone_No,
            string created_By,
            DateTime created_Date,
            string modified_By,
            DateTime modified_Date,
            bool isActive,
            bool isDeleted)
        {
            Teacher_Id = teacher_Id;
            User_Id = user_Id;
            FullName = fullName;
            Phone_No = phone_No;
            Created_By = created_By;
            Created_Date = created_Date;
            Modified_By = modified_By;
            Modified_Date = modified_Date;
            IsActive = isActive;
            IsDeleted = IsDeleted;
        }
    }
}

