using EduTrack.Models;

namespace EduTrack.Interfaces
{
    public interface IStudentClassService
    {
        List<StudentClass> GetAllStudentClasses();

        StudentClass GetStudentClassById(int id);

        void CreateStudentClass(StudentClass model);

        void UpdateStudentClass(StudentClass model);

        void DeleteStudentClass(int id);
    }
}