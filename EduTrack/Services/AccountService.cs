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
        public bool Register(RegisterViewModel newUser)
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
                    return false;

                int newUserId = Convert.ToInt32(result);

                return newUserId > 0;
            }
            catch
            {
                return false;
            }
        }
        public bool Authenticate(string email, string password)
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

                return dt.Rows.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}