using System.Data;
using Microsoft.Data.SqlClient;
using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.ViewModels;

namespace EduTrack.Services
{
    public class AccountService : IAccountService
    {
        private readonly DbHelper _db;

        public AccountService(DbHelper db)
        {
            _db = db;
        }

        public AuthenticationResult Register(RegisterViewModel newUser)
        {
            try
            {
                string hashedPassword = PasswordHelper.HashPassword(newUser.Password);

                var parameters = new[]
                {
                    new SqlParameter("@User_Name", newUser.FullName),
                    new SqlParameter("@PasswordHash", hashedPassword),
                    new SqlParameter("@Email", newUser.Email),
                    new SqlParameter("@PhoneNumber", newUser.PhoneNumber),
                    new SqlParameter("@Role_Id", DBNull.Value),
                    new SqlParameter("@Created_By", newUser.Email)
                };

                object? result = _db.ExecuteProcedureScalar("sp_User_Create", parameters);

                if (result == null)
                    return AuthenticationResult.Fail("An error occurred during registration. Please try again.");

                int newUserId = Convert.ToInt32(result);

                if (newUserId > 0)
                    return AuthenticationResult.Ok(newUserId);
                else
                    return AuthenticationResult.Fail("Failed to create user account. Please try again.");
            }
            catch (SqlException ex)
            {
                // Handle specific SQL errors
                if (ex.Message.Contains("Username already exists"))
                    return AuthenticationResult.Fail("This username is already registered. Please use a different username.");

                if (ex.Message.Contains("Email already exists"))
                    return AuthenticationResult.Fail("This email is already registered. Please use a different email or try logging in.");

                return AuthenticationResult.Fail("Registration failed. Please check your information and try again.");
            }
            catch (Exception ex)
            {
                return AuthenticationResult.Fail("An unexpected error occurred. Please try again later.");
            }
        }

        public AuthenticationResult Authenticate(string email, string password)
        {
            try
            {
                string hashedPassword = PasswordHelper.HashPassword(password);

                var parameters = new[]
                {
                    new SqlParameter("@EmailOrUsername", email),
                    new SqlParameter("@PasswordHash", hashedPassword)
                };

                DataTable dt = _db.ExecuteProcedure("sp_User_Authenticate", parameters);

                if (dt.Rows.Count > 0)
                {
                    int userId = Convert.ToInt32(dt.Rows[0]["User_Id"]);
                    return AuthenticationResult.Ok(userId);
                }

                // User not found - need to check why
                DataTable userCheck = _db.ExecuteProcedure("sp_User_GetByEmailOrUsername", new[]
                {
                    new SqlParameter("@EmailOrUsername", email)
                });

                if (userCheck.Rows.Count == 0)
                    return AuthenticationResult.Fail("Email or username not found. Please register first.");

                // User exists but password is wrong or account is inactive
                bool isActive = (bool)userCheck.Rows[0]["isActive"];
                bool isDeleted = (bool)userCheck.Rows[0]["isDeleted"];

                if (isDeleted)
                    return AuthenticationResult.Fail("Your account has been deleted. Please contact support.");

                if (!isActive)
                    return AuthenticationResult.Fail("Your account is inactive. Please contact the administrator.");

                return AuthenticationResult.Fail("Invalid email/username or password. Please try again.");
            }
            catch (Exception ex)
            {
                return AuthenticationResult.Fail("An error occurred during login. Please try again later.");
            }
        }
    }
}