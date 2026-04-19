using System.Data;
using Microsoft.Data.SqlClient;
using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.Models;

namespace EduTrack.Services
{
    public class ClassService : IClassService
    {
        private readonly DbHelper _db;

        public ClassService(DbHelper db)
        {
            _db = db;
        }

        // =============================
        // GET ALL
        // =============================
        public List<Class> GetAllClasses()
        {
            DataTable dt = _db.ExecuteProcedure("sp_Class_GetAll");

            List<Class> classes = new();

            foreach (DataRow row in dt.Rows)
            {
                classes.Add(Map(row));
            }

            return classes;
        }

        // =============================
        // GET BY ID
        // =============================
        public Class GetClassById(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Class_Id", id)
            };

            DataTable dt = _db.ExecuteProcedure("sp_Class_GetById", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return Map(dt.Rows[0]);
        }

        // =============================
        // CREATE
        // =============================
        public void CreateClass(Class c)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClassName", c.ClassName),
                new SqlParameter("@Section", c.Section),
                new SqlParameter("@Created_By", c.Created_By)
            };

            _db.ExecuteProcedureNonQuery("sp_Class_Create", parameters);
        }

        // =============================
        // UPDATE
        // =============================
        public void UpdateClass(Class c)
        {
            var parameters = new[]
            {
                new SqlParameter("@Class_Id", c.Class_Id),
                new SqlParameter("@ClassName", c.ClassName),
                new SqlParameter("@Section", c.Section),
                new SqlParameter("@Modified_By", c.Modified_By)
            };

            _db.ExecuteProcedureNonQuery("sp_Class_Update", parameters);
        }

        // =============================
        // DELETE
        // =============================
        public void DeleteClass(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Class_Id", id)
            };

            _db.ExecuteProcedureNonQuery("sp_Class_Delete", parameters);
        }

        // =============================
        // MAP
        // =============================
        private Class Map(DataRow row)
        {
            return new Class
            {
                Class_Id = Convert.ToInt32(row["Class_Id"]),
                ClassName = row["ClassName"]?.ToString() ?? "",
                Section = row["Section"]?.ToString() ?? "",
                Created_By = row["Created_By"]?.ToString() ?? "",
                Created_Date = row["Created_Date"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Created_Date"],
                Modified_By = row["Modified_By"]?.ToString() ?? "",
                Modified_Date = row["Modified_Date"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Modified_Date"],
                isActive = row["isActive"] == DBNull.Value ? false : (bool)row["isActive"],
                isDeleted = row["isDeleted"] == DBNull.Value ? false : (bool)row["isDeleted"]
            };
        }
    }
}