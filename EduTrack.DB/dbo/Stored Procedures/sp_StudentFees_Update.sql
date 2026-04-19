
CREATE PROCEDURE [dbo].[sp_StudentFees_Update]
    @StudentFees_Id INT,
    @Amount DECIMAL(10, 2),
    @PaymentStatus INT,
    @DueDate DATETIME,
    @PaymentMethod NVARCHAR(50),
    @Receipt_No NVARCHAR(100),
    @DiscountAmount DECIMAL(10, 2),
    @FineAmount DECIMAL(10, 2),
    @TotalAmount DECIMAL(10, 2),
    @Notes NVARCHAR(MAX),
    @Modified_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE [dbo].[StudentFees]
        SET 
            [Amount] = @Amount,
            [PaymentStatus] = @PaymentStatus,
            [DueDate] = @DueDate,
            [PaymentMethod] = @PaymentMethod,
            [Receipt_No] = @Receipt_No,
            [DiscountAmount] = @DiscountAmount,
            [FineAmount] = @FineAmount,
            [TotalAmount] = @TotalAmount,
            [Notes] = @Notes,
            [Modified_By] = @Modified_By,
            [Modified_Date] = GETDATE()
        WHERE [StudentFees_Id] = @StudentFees_Id AND [IsDeleted] = 0;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;