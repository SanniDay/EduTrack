using EduTrack.Models;

namespace EduTrack.Interfaces
{
    public interface IRoleService
    {
        List<Role> GetAll();

        Role? GetById(int id);

        void Create(Role role);

        void Update(Role role);

        void Delete(int id);
    }
}