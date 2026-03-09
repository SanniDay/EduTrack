
namespace EduTrack.Models
{
    public class Role
    {
        public int Role_Id { get; set; }

        public string Role_Name { get; set; } = string.Empty;

        public string Created_By { get; set; } = string.Empty;

        public DateTime Created_Date { get; set; }

        public string Modified_By { get; set; } = string.Empty;

        public DateTime Modified_Date { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public Role() { }

        public Role(
            int role_Id,
            string role_Name,
            string created_By,
            DateTime created_Date,
            string modified_By,
            DateTime modified_Date,
            bool isActive,
            bool isDeleted)
        {
            Role_Id = role_Id;
            Role_Name = role_Name;
            Created_By = created_By;
            Created_Date = created_Date;
            Modified_By = modified_By;
            Modified_Date = modified_Date;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}