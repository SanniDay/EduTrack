using EduTrack.ViewModels;

namespace EduTrack.Interfaces
{
    public interface IAccountService
    {
        bool Register(RegisterViewModel model);
        bool Authenticate(string userName, string password);
    }
}

