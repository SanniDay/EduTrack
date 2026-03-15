namespace EduTrack.Models
{
    public class TeacherClass
    {
        public int Teacher_Class_Id { get; set; }

        public int Teacher_Id { get; set; }

        public int Class_Id { get; set; }

        public string Subject { get; set; }

        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        public string Modified_By { get; set; }

        public DateTime Modified_Date { get; set; }

        public bool isActive { get; set; }

        public bool isDeleted { get; set; }
    }
}