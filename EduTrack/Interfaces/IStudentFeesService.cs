using EduTrack.Models;

namespace EduTrack.Interfaces
{
    public interface IStudentFeesService
    {
        List<StudentFees> GetAll();

        StudentFees? GetById(int id);

        List<StudentFees> GetByStudentId(int studentId);

        List<StudentFees> GetByFeesId(int feesId);

        void Create(StudentFees studentFees);

        void Update(StudentFees studentFees);

        void UpdateStatus(int studentFeesId, PaymentStatus status, DateTime? paidDate = null);

        void Delete(int id);
    }
}
