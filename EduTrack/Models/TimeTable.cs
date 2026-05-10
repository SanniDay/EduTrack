using System.ComponentModel.DataAnnotations;

namespace EduTrack.Models
{
    public class TimeTable
    {
        public int TimeTable_Id { get; set; }

        [Required]
        public int Class_Id { get; set; }

        public int? ClassSubject_Id { get; set; }

        public int? Teacher_Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Day_Name { get; set; } = string.Empty;

        [Required]
        public int Period_No { get; set; }

        [Required]
        public TimeSpan Start_Time { get; set; }

        [Required]
        public TimeSpan End_Time { get; set; }

        [StringLength(50)]
        public string Room_No { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public string Created_By { get; set; } = string.Empty;
        public DateTime Created_Date { get; set; } = DateTime.UtcNow;
        public string Modified_By { get; set; } = string.Empty;
        public DateTime Modified_Date { get; set; } = DateTime.UtcNow;

        public TimeTable() { }
    }
}
