
CREATE PROCEDURE [dbo].[sp_Fees_Delete]
    @Fees_Id INT,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE [dbo].[Fees]
        SET 
            [IsDeleted] = 1,
            [Modified_By] = @Modified_By,
            [Modified_Date] = GETDATE()
        WHERE [Fees_Id] = @Fees_Id;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;