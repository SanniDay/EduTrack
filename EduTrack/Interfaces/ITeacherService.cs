using EduTrack.Models;

namespace EduTrack.Services
{

    public interface ITeacherService
    {
        List<Teacher> GetAll();
        Teacher? GetById(int id);
        int Insert(Teacher teacher);
        int Update(Teacher teacher);
        int Delete(int id);
    }
}
