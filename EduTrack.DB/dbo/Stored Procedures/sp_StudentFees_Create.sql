
CREATE PROCEDURE [dbo].[sp_StudentFees_Create]
    @Student_Id INT,
    @Fees_Id INT,
    @Amount DECIMAL(10, 2),
    @PaymentStatus INT,
    @DueDate DATETIME,
    @PaymentMethod NVARCHAR(50),
    @DiscountAmount DECIMAL(10, 2),
    @FineAmount DECIMAL(10, 2),
    @TotalAmount DECIMAL(10, 2),
    @Notes NVARCHAR(MAX),
    @Created_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO [dbo].[StudentFees] 
            ([Student_Id], [Fees_Id], [Amount], [PaymentStatus], [DueDate], [PaymentMethod], 
             [DiscountAmount], [FineAmount], [TotalAmount], [Notes], [IsDeleted], [Created_By], [Created_Date])
        VALUES 
            (@Student_Id, @Fees_Id, @Amount, @PaymentStatus, @DueDate, @PaymentMethod, 
             @DiscountAmount, @FineAmount, @TotalAmount, @Notes, 0, @Created_By, GETDATE());
        
        SELECT SCOPE_IDENTITY() AS [StudentFees_Id];
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;