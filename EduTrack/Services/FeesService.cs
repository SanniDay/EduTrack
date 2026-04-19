using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.Helpers;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EduTrack.Services
{
    public class FeesService : IFeesService
    {
        private readonly DbHelper _db;

        public FeesService(DbHelper db)
        {
            _db = db;
        }

        public List<Fees> GetAll()
        {
            DataTable dt = _db.ExecuteProcedure("sp_Fees_GetAll");
            return ToList(dt);
        }

        public Fees? GetById(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Fees_Id", id)
            };

            DataTable dt = _db.ExecuteProcedure("sp_Fees_GetById", parameters);

            if (dt.Rows.Count == 0) return null;

            return Map(dt.Rows[0]);
        }

        public List<Fees> GetByClassId(int classId)
        {
            var parameters = new[]
            {
                new SqlParameter("@Class_Id", classId)
            };

            DataTable dt = _db.ExecuteProcedure("sp_Fees_GetByClassId", parameters);
            return ToList(dt);
        }

        public void Create(Fees fees)
        {
            var parameters = new[]
            {
                new SqlParameter("@Class_Id", fees.Class_Id),
                new SqlParameter("@FeeType", (int)fees.FeeType),
                new SqlParameter("@Amount", fees.Amount),
                new SqlParameter("@Currency", fees.Currency),
                new SqlParameter("@Description", fees.Description),
                new SqlParameter("@Created_By", fees.Created_By)
            };

            _db.ExecuteProcedureNonQuery("sp_Fees_Create", parameters);
        }

        public void Update(Fees fees)
        {
            var parameters = new[]
            {
                new SqlParameter("@Fees_Id", fees.Fees_Id),
                new SqlParameter("@Class_Id", fees.Class_Id),
                new SqlParameter("@FeeType", (int)fees.FeeType),
                new SqlParameter("@Amount", fees.Amount),
                new SqlParameter("@Currency", fees.Currency),
                new SqlParameter("@Description", fees.Description),
                new SqlParameter("@Modified_By", fees.Modified_By),
                new SqlParameter("@isActive", fees.IsActive)
            };

            _db.ExecuteProcedureNonQuery("sp_Fees_Update", parameters);
        }

        public void Delete(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Fees_Id", id),
                new SqlParameter("@Modified_By", "System")
            };

            _db.ExecuteProcedureNonQuery("sp_Fees_Delete", parameters);
        }

        private List<Fees> ToList(DataTable dt)
        {
            List<Fees> list = new();

            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));

            return list;
        }

        private Fees Map(DataRow row)
        {
            return new Fees(
                Convert.ToInt32(row["Fees_Id"]),
                Convert.ToInt32(row["Class_Id"]),
                (FeeType)Enum.Parse(typeof(FeeType), row["FeeType"]?.ToString() ?? "1"),
                Convert.ToDecimal(row["Amount"]),
                row["Currency"]?.ToString() ?? "USD",
                row["Description"]?.ToString() ?? "",
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
