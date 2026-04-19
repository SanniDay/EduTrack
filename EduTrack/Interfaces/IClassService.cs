using EduTrack.Models;

namespace EduTrack.Interfaces
{
    public interface IClassService
    {
        List<Class> GetAllClasses();

        Class GetClassById(int id);

        void CreateClass(Class model);

        void UpdateClass(Class model);

        void DeleteClass(int id);
    }
}