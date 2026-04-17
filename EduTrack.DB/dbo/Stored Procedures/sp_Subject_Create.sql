
CREATE PROCEDURE [dbo].[sp_Subject_Create]
    @Subject_Name NVARCHAR(100),
    @Subject_Code NVARCHAR(20),
    @Description NVARCHAR(MAX),
    @Created_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO [dbo].[Subjects] 
            ([Subject_Name], [Subject_Code], [Description], [IsActive], [IsDeleted], [Created_By], [Created_Date])
        VALUES 
            (@Subject_Name, @Subject_Code, @Description, 1, 0, @Created_By, GETDATE());
        
        SELECT SCOPE_IDENTITY() AS [Subject_Id];
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;