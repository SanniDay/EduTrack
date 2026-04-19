
CREATE PROCEDURE [dbo].[sp_Fees_GetById]
    @Fees_Id INT
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
    WHERE [Fees_Id] = @Fees_Id AND [IsDeleted] = 0;
END;