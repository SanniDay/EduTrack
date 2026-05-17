namespace EduTrack.Models
{
    public class ClassSubject
    {
        public int ClassSubject_Id { get; set; }

        public int Class_Id { get; set; }

        public int Subject_Id { get; set; }

        public string SubjectName { get; set; }

        public bool IsCore { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        public string Modified_By { get; set; }

        public DateTime Modified_Date { get; set; }
    }
}
