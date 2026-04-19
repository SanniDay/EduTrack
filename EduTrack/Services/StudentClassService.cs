using System.Data;
using Microsoft.Data.SqlClient;
using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.Models;

namespace EduTrack.Services
{
    public class StudentClassService : IStudentClassService
    {
        private readonly DbHelper _db;

        public StudentClassService(DbHelper db)
        {
            _db = db;
        }

        // =============================
        // GET ALL
        // =============================
        public List<StudentClass> GetAllStudentClasses()
        {
            DataTable dt = _db.ExecuteProcedure("sp_StudentClass_GetAll");

            List<StudentClass> list = new();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(Map(row));
            }

            return list;
        }

        // =============================
        // GET BY ID
        // =============================
        public StudentClass? GetStudentClassById(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Student_Class_Id", id)
            };

            DataTable dt = _db.ExecuteProcedure("sp_StudentClass_GetById", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return Map(dt.Rows[0]);
        }

        // =============================
        // CREATE
        // =============================
        public void CreateStudentClass(StudentClass sc)
        {
            var parameters = new[]
            {
                new SqlParameter("@Student_Id", sc.Student_Id),
                new SqlParameter("@Class_Id", sc.Class_Id),
                new SqlParameter("@Created_By", sc.Created_By)
            };

            _db.ExecuteProcedureNonQuery("sp_StudentClass_Create", parameters);
        }

        // =============================
        // UPDATE
        // =============================
        public void UpdateStudentClass(StudentClass sc)
        {
            var parameters = new[]
            {
                new SqlParameter("@Student_Class_Id", sc.Student_Class_Id),
                new SqlParameter("@Student_Id", sc.Student_Id),
                new SqlParameter("@Class_Id", sc.Class_Id),
                new SqlParameter("@Modified_By", sc.Modified_By)
            };

            _db.ExecuteProcedureNonQuery("sp_StudentClass_Update", parameters);
        }

        // =============================
        // DELETE
        // =============================
        public void DeleteStudentClass(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Student_Class_Id", id)
            };

            _db.ExecuteProcedureNonQuery("sp_StudentClass_Delete", parameters);
        }

        // =============================
        // MAP
        // =============================
        private StudentClass Map(DataRow row)
        {
            return new StudentClass
            {
                Student_Class_Id = Convert.ToInt32(row["Student_Class_Id"]),
                Student_Id = Convert.ToInt32(row["Student_Id"]),
                Class_Id = Convert.ToInt32(row["Class_Id"]),
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