CREATE PROCEDURE [dbo].[sp_Attendance_Delete]
    @Attendance_Id INT
AS
BEGIN
    UPDATE Attendance
    SET IsDeleted = 1, IsActive = 0, Modified_Date = GETDATE()
    WHERE Attendance_Id = @Attendance_Id;
END