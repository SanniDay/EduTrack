
CREATE PROCEDURE [dbo].[sp_StudentFees_GetByFeesId]
    @Fees_Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        [StudentFees_Id],
        [Student_Id],
        [Fees_Id],
        [Amount],
        [PaymentStatus],
        [DueDate],
        [PaidDate],
        [PaymentMethod],
        [Receipt_No],
        [DiscountAmount],
        [FineAmount],
        [TotalAmount],
        [Notes],
        [IsDeleted],
        [Created_By],
        [Created_Date],
        [Modified_By],
        [Modified_Date]
    FROM [dbo].[StudentFees]
    WHERE [Fees_Id] = @Fees_Id AND [IsDeleted] = 0
    ORDER BY [Student_Id];
END;