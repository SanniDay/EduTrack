
-- =====================================================
-- STUDENT FEES STORED PROCEDURES (7 procedures)
-- =====================================================

CREATE PROCEDURE [dbo].[sp_StudentFees_GetAll]
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
    WHERE [IsDeleted] = 0
    ORDER BY [DueDate] DESC;
END;