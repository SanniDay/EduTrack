
CREATE PROCEDURE [dbo].[sp_Subject_GetById]
    @Subject_Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        [Subject_Id],
        [Subject_Name],
        [Subject_Code],
        [Description],
        [IsActive],
        [IsDeleted],
        [Created_By],
        [Created_Date],
        [Modified_By],
        [Modified_Date]
    FROM [dbo].[Subjects]
    WHERE [Subject_Id] = @Subject_Id AND [IsDeleted] = 0;
END;