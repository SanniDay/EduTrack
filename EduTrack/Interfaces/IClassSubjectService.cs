using EduTrack.Models;

namespace EduTrack.Interfaces
{
    public interface IClassSubjectService
    {
        List<ClassSubject> GetAllClassSubjects();
        ClassSubject? GetClassSubjectById(int id);
        void CreateClassSubject(ClassSubject model);
        void UpdateClassSubject(ClassSubject model);
        void DeleteClassSubject(int id);
    }
}
