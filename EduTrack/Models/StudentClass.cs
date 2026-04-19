namespace EduTrack.Models
{
    public class StudentClass
    {
            public int Student_Class_Id { get; set; }

            public int Student_Id { get; set; }

            public int Class_Id { get; set; }

            public string Created_By { get; set; }

            public DateTime Created_Date { get; set; }

            public string Modified_By { get; set; }

            public DateTime Modified_Date { get; set; }

            public bool isActive { get; set; }

            public bool isDeleted { get; set; }
        }
    }

