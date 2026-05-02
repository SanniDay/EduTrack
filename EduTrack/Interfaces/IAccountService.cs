using EduTrack.ViewModels;

namespace EduTrack.Interfaces
{
    public interface IAccountService
    {
        AuthenticationResult Register(RegisterViewModel model);
        AuthenticationResult Authenticate(string userName, string password);
    }
}

