
-- =====================================================
-- SUBJECT STORED PROCEDURES (5 procedures)
-- =====================================================

CREATE PROCEDURE [dbo].[sp_Subject_GetAll]
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
    WHERE [IsDeleted] = 0
    ORDER BY [Subject_Name];
END;