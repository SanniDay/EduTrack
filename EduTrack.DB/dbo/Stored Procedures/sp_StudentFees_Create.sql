
CREATE PROCEDURE [dbo].[sp_StudentFees_Create]
    @Student_Id INT,
    @Fees_Id INT,
    @Amount DECIMAL(10, 2),
    @DiscountAmount DECIMAL(10, 2) = 0,
    @FineAmount DECIMAL(10, 2) = 0,
    @PaymentStatus INT = 1,
    @PaidDate DATETIME,
    @DueDate DATETIME,
    @PaymentMethod NVARCHAR(50) = NULL,
    @Notes NVARCHAR(MAX) = NULL,
    @Created_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @TotalAmount DECIMAL(10, 2) = @Amount - ISNULL(@DiscountAmount, 0) + ISNULL(@FineAmount, 0);
        DECLARE @Receipt_No NVARCHAR(100) = 'RCP-' + FORMAT(GETDATE(), 'yyyyMMddHHmmss') + '-' + CAST(@Student_Id AS NVARCHAR(10));

        INSERT INTO [dbo].[StudentFees] 
            ([Student_Id], [Fees_Id], [Amount], [PaymentStatus], [DueDate], [PaidDate], [PaymentMethod], 
             [DiscountAmount], [FineAmount], [TotalAmount], [Receipt_No], [Notes], [IsDeleted], [Created_By], [Created_Date])
        VALUES 
            (@Student_Id, @Fees_Id, @Amount, @PaymentStatus, @DueDate, @PaidDate, @PaymentMethod, 
             @DiscountAmount, @FineAmount, @TotalAmount, @Receipt_No, @Notes, 0, @Created_By, GETDATE());
        
        SELECT SCOPE_IDENTITY() AS [StudentFees_Id];
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;