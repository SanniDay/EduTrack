
/* =========================================================
   ATTENDANCE DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_Attendance_Delete]
    @Attendance_Id INT
AS
BEGIN
    DELETE FROM Attendance
    WHERE Attendance_Id = @Attendance_Id
END