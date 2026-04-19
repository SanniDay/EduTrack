
CREATE PROCEDURE [dbo].[sp_StudentFees_GetByStudentId]
    @Student_Id INT
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
    WHERE [Student_Id] = @Student_Id AND [IsDeleted] = 0
    ORDER BY [DueDate] DESC;
END;