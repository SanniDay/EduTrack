using EduTrack.Models;

namespace EduTrack.Interfaces
{
    public interface  IStudentService
    {
        List<Student> GetAll();
        Student? GetById(int id);
        int Insert(Student student);
        int Update(Student student);
        int Delete(int id);
    }
}
