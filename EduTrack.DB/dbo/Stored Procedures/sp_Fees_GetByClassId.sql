
CREATE PROCEDURE [dbo].[sp_Fees_GetByClassId]
    @Class_Id INT
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
    WHERE [Class_Id] = @Class_Id AND [IsDeleted] = 0
    ORDER BY [FeeType];
END;