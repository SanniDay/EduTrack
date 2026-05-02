
CREATE PROCEDURE [dbo].[sp_StudentFees_UpdateStatus]
    @StudentFees_Id INT,
    @PaymentStatus INT,
    @PaidDate DATETIME = NULL,
    @PaymentMethod NVARCHAR(50) = NULL,
    @Receipt_No NVARCHAR(100) = NULL,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Auto-fill PaidDate if PaymentStatus = 2 (Paid) and PaidDate is NULL
        IF @PaymentStatus = 2 AND @PaidDate IS NULL
            SET @PaidDate = GETDATE();

        UPDATE [dbo].[StudentFees]
        SET 
            [PaymentStatus] = @PaymentStatus,
            [PaidDate] = @PaidDate,
            [PaymentMethod] = @PaymentMethod,
            [Receipt_No] = @Receipt_No,
            [Modified_By] = @Modified_By,
            [Modified_Date] = GETDATE()
        WHERE [StudentFees_Id] = @StudentFees_Id AND [IsDeleted] = 0;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;