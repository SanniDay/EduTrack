using System.Data;
using Microsoft.Data.SqlClient;
using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.Models;

namespace EduTrack.Services
{
    public class TeacherClassService : ITeacherClassService
    {
        private readonly DbHelper _db;

        public TeacherClassService(DbHelper db)
        {
            _db = db;
        }

        // =============================
        // GET ALL
        // =============================
        public List<TeacherClass> GetAllTeacherClasses()
        {
            DataTable dt = _db.ExecuteProcedure("sp_TeacherClass_GetAll");

            List<TeacherClass> list = new();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(Map(row));
            }

            return list;
        }

        // =============================
        // GET BY ID
        // =============================
        public TeacherClass? GetTeacherClassById(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Teacher_Class_Id", id)
            };

            DataTable dt = _db.ExecuteProcedure("sp_TeacherClass_GetById", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return Map(dt.Rows[0]);
        }

        // =============================
        // CREATE
        // =============================
        public void CreateTeacherClass(TeacherClass tc)
        {
            var parameters = new[]
            {
                new SqlParameter("@Teacher_Id", tc.Teacher_Id),
                new SqlParameter("@Class_Id", tc.Class_Id),
                new SqlParameter("@Subject", tc.Subject),
                new SqlParameter("@Created_By", tc.Created_By)
            };

            _db.ExecuteProcedureNonQuery("sp_TeacherClass_Create", parameters);
        }

        // =============================
        // UPDATE
        // =============================
        public void UpdateTeacherClass(TeacherClass tc)
        {
            var parameters = new[]
            {
                new SqlParameter("@Teacher_Class_Id", tc.Teacher_Class_Id),
                new SqlParameter("@Teacher_Id", tc.Teacher_Id),
                new SqlParameter("@Class_Id", tc.Class_Id),
                new SqlParameter("@Subject", tc.Subject),
                new SqlParameter("@Modified_By", tc.Modified_By)
            };

            _db.ExecuteProcedureNonQuery("sp_TeacherClass_Update", parameters);
        }

        // =============================
        // DELETE
        // =============================
        public void DeleteTeacherClass(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Teacher_Class_Id", id)
            };

            _db.ExecuteProcedureNonQuery("sp_TeacherClass_Delete", parameters);
        }

        // =============================
        // MAP
        // =============================
        private TeacherClass Map(DataRow row)
        {
            return new TeacherClass
            {
                Teacher_Class_Id = Convert.ToInt32(row["Teacher_Class_Id"]),
                Teacher_Id = Convert.ToInt32(row["Teacher_Id"]),
                Class_Id = Convert.ToInt32(row["Class_Id"]),
                Subject = row["Subject"]?.ToString() ?? "",
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