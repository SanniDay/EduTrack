using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.Helpers;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EduTrack.Services
{
    public class UserService : IUserService
    {
        private readonly DbHelper _db;

        public UserService(DbHelper db)
        {
            _db = db;
        }

        // =============================
        // GET ALL
        // =============================
        public List<User> GetAll()
        {
            DataTable dt = _db.ExecuteProcedure("sp_User_GetAll");
            return ToList(dt);
        }

        // =============================
        // GET BY ID
        // =============================
        public User? GetById(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@User_Id", id)

            };

            DataTable dt = _db.ExecuteProcedure("sp_User_GetById", parameters);

            if (dt.Rows.Count == 0) return null;

            return Map(dt.Rows[0]);
        }

        // =============================
        // INSERT
        // =============================
        public void Create(User u)
        {
            var parameters = new[]
            {
                new SqlParameter("@User_Name", u.User_Name),
                new SqlParameter("@PasswordHash", u.PasswordHash),
                new SqlParameter("@Email", u.Email),
                new SqlParameter("@Role_Id", (object?)u.Role_Id ?? DBNull.Value),
                new SqlParameter("@Created_By", u.Created_By)
            };

            _db.ExecuteProcedureNonQuery("sp_User_Create", parameters);
        }

        // =============================
        // UPDATE
        // =============================
        public void Update(User u)
        {
            var parameters = new[]
            {
                new SqlParameter("@User_Id", u.User_Id),
                new SqlParameter("@User_Name", u.User_Name),
                new SqlParameter("@Email", u.Email),
                new SqlParameter("@Role_Id", (object?)u.Role_Id ?? DBNull.Value),
                new SqlParameter("@Modified_By", u.Modified_By),
                new SqlParameter("@isActive", u.IsActive)
            };

            _db.ExecuteProcedureNonQuery("sp_User_Update", parameters);
        }

        // =============================
        // DELETE
        // =============================
        public void Delete(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@User_Id", id),
                new SqlParameter("@Modified_By", "System") // or pass logged-in user
            };

            _db.ExecuteProcedureNonQuery("sp_User_Delete", parameters);
        }

        // =============================
        // DATATABLE → LIST
        // =============================
        private List<User> ToList(DataTable dt)
        {
            List<User> list = new();

            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));

            return list;
        }

        // =============================
        // MAP
        // =============================
        private User Map(DataRow row)
        {
            return new User(
                Convert.ToInt32(row["User_Id"]),
                row["User_Name"]?.ToString() ?? "",
                row["PasswordHash"]?.ToString() ?? "",
                row["Email"]?.ToString() ?? "",
                row["Role_Id"] == DBNull.Value ? null : (int?)row["Role_Id"],
                row["Created_By"]?.ToString() ?? "",
                row["Created_Date"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Created_Date"],
                row["Modified_By"]?.ToString() ?? "",
                row["Modified_Date"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Modified_Date"],
                row["isActive"] == DBNull.Value ? false : (bool)row["isActive"],
                row["isDeleted"] == DBNull.Value ? false : (bool)row["isDeleted"]
            );
        }
    }
}