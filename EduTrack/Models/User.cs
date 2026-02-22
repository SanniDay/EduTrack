namespace EduTrack.Models
{
    public class User
    {
        public int User_Id { get; set; }

        public string User_Name { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int? Role_Id { get; set; }

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; }

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public User() { }

        public User(
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
