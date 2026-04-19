namespace EduTrack.Models
{
    public class Attendance
    {
        public int Attendance_Id { get; set; }

        public int Student_Class_Id { get; set; }

        public int? ClassSubject_Id { get; set; }

        public DateTime Attendance_Date { get; set; }

        public string Status { get; set; }

        public int? Marked_By_Teacher_Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        public string Modified_By { get; set; }

        public DateTime Modified_Date { get; set; }
    }
}
