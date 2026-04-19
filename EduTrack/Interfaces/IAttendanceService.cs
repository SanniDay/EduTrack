using EduTrack.Models;

namespace EduTrack.Interfaces
{
    public interface IAttendanceService
    {
        List<Attendance> GetAllAttendance();
        Attendance? GetAttendanceById(int id);
        void CreateAttendance(Attendance model);
        void UpdateAttendance(Attendance model);
        void DeleteAttendance(int id);
    }
}
