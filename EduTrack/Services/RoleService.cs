using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.Helpers;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EduTrack.Services
{
    public class RoleService : IRoleService
    {
        private readonly DbHelper _db;

        public RoleService(DbHelper db)
        {
            _db = db;
        }

        // GET ALL
        public List<Role> GetAll()
        {
            DataTable dt = _db.ExecuteProcedure("sp_Role_GetAll");
            return ToList(dt);
        }

        // GET BY ID
        public Role? GetById(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Role_Id", id)
            };

            DataTable dt = _db.ExecuteProcedure("sp_Role_GetById", parameters);

            if (dt.Rows.Count == 0) return null;

            return Map(dt.Rows[0]);
        }

        // CREATE
        public void Create(Role role)
        {
            var parameters = new[]
            {
                new SqlParameter("@Role_Name", role.Role_Name),
                new SqlParameter("@Created_By", role.Created_By)
            };

            _db.ExecuteProcedureNonQuery("sp_Role_Create", parameters);
        }

        // UPDATE
        public void Update(Role role)
        {
            var parameters = new[]
            {
                new SqlParameter("@Role_Id", role.Role_Id),
                new SqlParameter("@Role_Name", role.Role_Name),
                new SqlParameter("@Modified_By", role.Modified_By),
                new SqlParameter("@isActive", role.IsActive)
            };

            _db.ExecuteProcedureNonQuery("sp_Role_Update", parameters);
        }

        // DELETE
        public void Delete(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Role_Id", id),
                new SqlParameter("@Modified_By", "System")
            };

            _db.ExecuteProcedureNonQuery("sp_Role_Delete", parameters);
        }

        // Helpers
        private List<Role> ToList(DataTable dt)
        {
            List<Role> list = new();

            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));

            return list;
        }

        private Role Map(DataRow row)
        {
            return new Role(
                Convert.ToInt32(row["Role_Id"]),
                row["Role_Name"]?.ToString() ?? "",
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