
CREATE PROCEDURE [dbo].[sp_Attendance_Update]
    @Attendance_Id INT,
    @Student_Class_Id INT,
    @ClassSubject_Id INT = NULL,
    @Attendance_Date DATE,
    @Status VARCHAR(10),
    @Marked_By_Teacher_Id INT = NULL,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE [dbo].[Attendance]
        SET 
            [Student_Class_Id] = @Student_Class_Id,
            [ClassSubject_Id] = @ClassSubject_Id,
            [Attendance_Date] = @Attendance_Date,
            [Status] = @Status,
            [Marked_By_Teacher_Id] = @Marked_By_Teacher_Id,
            [Modified_By] = @Modified_By,
            [Modified_Date] = GETDATE()
        WHERE [Attendance_Id] = @Attendance_Id AND [IsDeleted] = 0;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;