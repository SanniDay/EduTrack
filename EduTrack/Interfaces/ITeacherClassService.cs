using EduTrack.Models;

namespace EduTrack.Interfaces
{
    public interface ITeacherClassService
    {
        List<TeacherClass> GetAllTeacherClasses();

        TeacherClass GetTeacherClassById(int id);

        void CreateTeacherClass(TeacherClass model);

        void UpdateTeacherClass(TeacherClass model);

        void DeleteTeacherClass(int id);
    }
}