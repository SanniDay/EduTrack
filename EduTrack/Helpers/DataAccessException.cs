using System;

namespace EduTrack.Helpers
{
    // Custom exception to wrap SQL errors and provide a user-friendly message
    public class DataAccessException : Exception
    {
        public string ProcedureName { get; }

        public DataAccessException(string message, Exception innerException, string procedureName = "")
            : base(message, innerException)
        {
            ProcedureName = procedureName ?? string.Empty;
        }
    }
}
