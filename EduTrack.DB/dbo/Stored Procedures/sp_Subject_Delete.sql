
CREATE PROCEDURE [dbo].[sp_Subject_Delete]
    @Subject_Id INT,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE [dbo].[Subjects]
        SET 
            [IsDeleted] = 1,
            [Modified_By] = @Modified_By,
            [Modified_Date] = GETDATE()
        WHERE [Subject_Id] = @Subject_Id;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;