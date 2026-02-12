using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.Helpers;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EduTrack.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly DbHelper _db;

        public TeacherService(DbHelper db)
        {
            _db = db;
        }

        public List<Teacher> GetAll()
        {
            DataTable dt = _db.ExecuteProcedure("sp_Teacher_GetAll");
            return ToList(dt);
        }

        public Teacher? GetById(int id)
        {
            var param = new[] { new SqlParameter("@Teacher_Id", id) };

            DataTable dt = _db.ExecuteProcedure("sp_Teacher_GetById", param);

            if (dt.Rows.Count == 0) return null;

            return Map(dt.Rows[0]);
        }

        public int Insert(Teacher t)
        {
            return _db.ExecuteNonQuery("sp_Teacher_Insert", Params(t));
        }

        public int Update(Teacher t)
        {
            var list = Params(t).ToList();
            list.Add(new SqlParameter("@Teacher_Id", t.Teacher_Id));

            return _db.ExecuteNonQuery("sp_Teacher_Update", list.ToArray());
        }

        public int Delete(int id)
        {
            var param = new[] { new SqlParameter("@Teacher_Id", id) };
            return _db.ExecuteNonQuery("sp_Teacher_Delete", param);
        }

        private List<Teacher> ToList(DataTable dt)
        {
            List<Teacher> list = new();

            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));

            return list;
        }

        private Teacher Map(DataRow row)
        {
            return new Teacher(
                Convert.ToInt32(row["Teacher_Id"]),
                row["User_Id"] == DBNull.Value ? null : (int?)row["User_Id"],
                row["FullName"]?.ToString(),
                row["Phone_No"]?.ToString(),
                row["Created_By"]?.ToString() ?? "",
                row["Created_Date"] == DBNull.Value ? null : (DateTime?)row["Created_Date"],
                row["Modified_By"]?.ToString() ?? "",
                row["Modified_Date"] == DBNull.Value ? null : (DateTime?)row["Modified_Date"],
                row["IsActive"] == DBNull.Value ? null : (bool?)row["IsActive"],
                row["IsDeleted"] == DBNull.Value ? null : (bool?)row["IsDeleted"]
            );
        }

        private SqlParameter[] Params(Teacher t)
        {
            return new[]
            {
                new SqlParameter("@User_Id", (object?)t.User_Id ?? DBNull.Value),
                new SqlParameter("@FullName", (object?)t.FullName ?? DBNull.Value),
                new SqlParameter("@Phone_No", (object?)t.Phone_No ?? DBNull.Value),
                new SqlParameter("@Created_By", t.Created_By),
                new SqlParameter("@Modified_By", t.Modified_By),
                new SqlParameter("@IsActive", (object?)t.IsActive ?? DBNull.Value),
                new SqlParameter("@IsDeleted", (object?)t.IsDeleted ?? DBNull.Value),
            };
        }
    }
}
