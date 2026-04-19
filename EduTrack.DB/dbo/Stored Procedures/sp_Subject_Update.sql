
CREATE PROCEDURE [dbo].[sp_Subject_Update]
    @Subject_Id INT,
    @Subject_Name NVARCHAR(100),
    @Subject_Code NVARCHAR(20),
    @Description NVARCHAR(MAX),
    @IsActive BIT,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE [dbo].[Subjects]
        SET 
            [Subject_Name] = @Subject_Name,
            [Subject_Code] = @Subject_Code,
            [Description] = @Description,
            [IsActive] = @IsActive,
            [Modified_By] = @Modified_By,
            [Modified_Date] = GETDATE()
        WHERE [Subject_Id] = @Subject_Id AND [IsDeleted] = 0;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;