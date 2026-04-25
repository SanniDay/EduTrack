
CREATE PROCEDURE [dbo].[sp_ClassSubject_Delete]
    @ClassSubject_Id INT,
    @Modified_By NVARCHAR(100) = 'System'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE [dbo].[ClassSubject]
        SET 
            [IsDeleted] = 1,
            [IsActive] = 0,
            [Modified_By] = @Modified_By,
            [Modified_Date] = GETDATE()
        WHERE [ClassSubject_Id] = @ClassSubject_Id;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;