using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EduTrack.Services
{
    public class StudentService:IStudentService
    {
        private readonly DbHelper _db;

        public StudentService(DbHelper db)
        {
            _db = db;
        }

        public List<Student> GetAll()
        {
            DataTable dt = _db.ExecuteProcedure("sp_Student_GetAll");
            return ToList(dt);
        }

        public Student? GetById(int id)
        {
            var param = new[] { new SqlParameter("@Student_Id", id) };

            DataTable dt = _db.ExecuteProcedure("sp_Student_GetById", param);

            if (dt.Rows.Count == 0) return null;

            return Map(dt.Rows[0]);
        }

        public int Insert(Student s)
        {
            return _db.ExecuteNonQuery("sp_Student_Insert", Params(s));
        }

        public int Update(Student s)
        {
            var list = Params(s).ToList();
            list.Add(new SqlParameter("@Student_Id", s.Student_Id));

            return _db.ExecuteNonQuery("sp_Student_Update", list.ToArray());
        }

        public int Delete(int id)
        {
            var param = new[] { new SqlParameter("@Student_Id", id) };
            return _db.ExecuteNonQuery("sp_Student_Delete", param);
        }

        private List<Student> ToList(DataTable dt)
        {
            List<Student> list = new();

            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));

            return list;
        }
        private Student Map(DataRow row)
        {
            return new Student(
                Convert.ToInt32(row["Student_Id"]),
                row["User_Id"] as int?,
                row["FullName"]?.ToString(),
                row["DOB"] as DateTime?,
                Enum.TryParse<Gender>(row["Gender"]?.ToString(), out var g) ? g : default,
                row["Phone_No"]?.ToString(),
                row["Address"]?.ToString(),
                row["Created_By"]?.ToString() ?? "",
                row["Created_Date"] as DateTime?,
                row["Modified_By"]?.ToString() ?? "",
                row["Modified_Date"] as DateTime?,
                row["IsActive"] as bool?,
                row["IsDeleted"] as bool?
            );
        }
        private SqlParameter[] Params(Student s)
        {
            return new[]
            {
                new SqlParameter("@User_Id", (object?)s.User_Id ?? DBNull.Value),
                new SqlParameter("@FullName", (object?)s.FullName ?? DBNull.Value),
                new SqlParameter("@DOB", (object?)s.DOB ?? DBNull.Value),
                new SqlParameter("@Gender", (object?)s.Gender.ToString() ?? DBNull.Value),
                new SqlParameter("@Phone_No", (object?)s.Phone_No ?? DBNull.Value),
                new SqlParameter("@Address", (object?)s.Address ?? DBNull.Value),
                new SqlParameter("@Created_By", s.Created_By),
                new SqlParameter("@Modified_By", s.Modified_By),
                new SqlParameter("@isActive", (object?)s.IsActive ?? DBNull.Value),
            };
        }

    }
}
