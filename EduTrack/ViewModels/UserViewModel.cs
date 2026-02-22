using System.ComponentModel.DataAnnotations;

namespace EduTrack.ViewModels
{
    public class UserViewModel
    {
        public int User_Id { get; set; }

        [Required(ErrorMessage = "User name is required")]
        [StringLength(50, ErrorMessage = "User name cannot exceed 50 characters")]
        public string User_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        public int? Role_Id { get; set; }

    
        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; }

        
        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public UserViewModel() { }

        public UserViewModel(
            int user_Id,
            string user_Name,
            string passwordHash,
            string email,
            int? role_Id,
            string created_By,
            DateTime created_Date,
            string modified_By,
            DateTime modified_Date,
            bool isActive,
            bool isDeleted)
        {
            User_Id = user_Id;
            User_Name = user_Name;
            PasswordHash = passwordHash;
            Email = email;
            Role_Id = role_Id;
            Created_By = created_By;
            Created_Date = created_Date;
            Modified_By = modified_By;
            Modified_Date = modified_Date;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}
