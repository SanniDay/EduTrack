
CREATE PROCEDURE [dbo].[sp_StudentFees_Update]
    @StudentFees_Id INT,
    @Amount DECIMAL(10, 2),
    @DiscountAmount DECIMAL(10, 2) = 0,
    @FineAmount DECIMAL(10, 2) = 0,
    @PaymentStatus INT,
    @DueDate DATETIME,
    @PaymentMethod NVARCHAR(50) = NULL,
    @Receipt_No NVARCHAR(100) = NULL,
    @Notes NVARCHAR(MAX) = NULL,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @TotalAmount DECIMAL(10, 2) = @Amount - ISNULL(@DiscountAmount, 0) + ISNULL(@FineAmount, 0);

        UPDATE [dbo].[StudentFees]
        SET 
            [Amount] = @Amount,
            [DiscountAmount] = @DiscountAmount,
            [FineAmount] = @FineAmount,
            [TotalAmount] = @TotalAmount,
            [PaymentStatus] = @PaymentStatus,
            [DueDate] = @DueDate,
            [PaymentMethod] = @PaymentMethod,
            [Receipt_No] = @Receipt_No,
            [Notes] = @Notes,
            [Modified_By] = @Modified_By,
            [Modified_Date] = GETDATE()
        WHERE [StudentFees_Id] = @StudentFees_Id AND [IsDeleted] = 0;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;