using EduTrack.Models;

namespace EduTrack.Interfaces
{
    public interface ITimeTableService
    {
        List<TimeTable> GetAll();
        TimeTable? GetById(int id);
        List<TimeTable> GetByClass(int classId);
        int Create(TimeTable t);
        int Update(TimeTable t);
        int Delete(int id);
        int Restore(int id);
    }
}
