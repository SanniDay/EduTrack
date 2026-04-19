
-- =====================================================
-- FEES STORED PROCEDURES (6 procedures)
-- =====================================================

CREATE PROCEDURE [dbo].[sp_Fees_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        [Fees_Id],
        [Class_Id],
        [FeeType],
        [Amount],
        [Currency],
        [Description],
        [DueDate],
        [IsActive],
        [IsDeleted],
        [Created_By],
        [Created_Date],
        [Modified_By],
        [Modified_Date]
    FROM [dbo].[Fees]
    WHERE [IsDeleted] = 0
    ORDER BY [Class_Id], [FeeType];
END;