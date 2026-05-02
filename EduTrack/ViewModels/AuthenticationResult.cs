namespace EduTrack.ViewModels
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; }

        public AuthenticationResult()
        {
            Success = false;
            Message = "An error occurred.";
            UserId = null;
        }

        public static AuthenticationResult Ok(int userId)
        {
            return new AuthenticationResult { Success = true, Message = "Success", UserId = userId };
        }

        public static AuthenticationResult Fail(string message)
        {
            return new AuthenticationResult { Success = false, Message = message };
        }
    }
}
