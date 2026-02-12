using System.Data;
using Microsoft.Data.SqlClient;

namespace EduTrack.Helpers
{
    public class DbHelper
    {
        private readonly string _connectionString;

        public DbHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new Exception("Connection string not found.");
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }


        public DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            using SqlConnection con = GetConnection();
            using SqlCommand cmd = new SqlCommand(query, con);

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public object? ExecuteScalar(string query, SqlParameter[]? parameters = null)
        {
            using SqlConnection con = GetConnection();
            using SqlCommand cmd = new SqlCommand(query, con);

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            con.Open();
            return cmd.ExecuteScalar();
        }


        public int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            using SqlConnection con = GetConnection();
            using SqlCommand cmd = new SqlCommand(query, con);

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            con.Open();
            return cmd.ExecuteNonQuery();
        }


        public DataTable ExecuteProcedure(string procedureName, SqlParameter[]? parameters = null)
        {
            using SqlConnection con = GetConnection();
            using SqlCommand cmd = new SqlCommand(procedureName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


    }
}
