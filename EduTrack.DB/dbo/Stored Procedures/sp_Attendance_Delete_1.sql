
CREATE PROCEDURE [dbo].[sp_Attendance_Delete]
    @Attendance_Id INT,
    @Modified_By NVARCHAR(100) = 'System'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE [dbo].[Attendance]
        SET 
            [IsDeleted] = 1,
            [IsActive] = 0,
            [Modified_By] = @Modified_By,
            [Modified_Date] = GETDATE()
        WHERE [Attendance_Id] = @Attendance_Id;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;