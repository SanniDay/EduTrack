using EduTrack.Models;

namespace EduTrack.Interfaces
{
    public interface ISubjectService
    {
        List<Subject> GetAll();

        Subject? GetById(int id);

        void Create(Subject subject);

        void Update(Subject subject);

        void Delete(int id);
    }
}
