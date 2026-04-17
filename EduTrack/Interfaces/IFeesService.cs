using EduTrack.Models;

namespace EduTrack.Interfaces
{
    public interface IFeesService
    {
        List<Fees> GetAll();

        Fees? GetById(int id);

        List<Fees> GetByClassId(int classId);

        void Create(Fees fees);

        void Update(Fees fees);

        void Delete(int id);
    }
}
