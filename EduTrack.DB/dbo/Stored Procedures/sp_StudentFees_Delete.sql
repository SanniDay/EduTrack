
CREATE PROCEDURE [dbo].[sp_StudentFees_Delete]
    @StudentFees_Id INT,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE [dbo].[StudentFees]
        SET 
            [IsDeleted] = 1,
            [Modified_By] = @Modified_By,
            [Modified_Date] = GETDATE()
        WHERE [StudentFees_Id] = @StudentFees_Id;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;