
CREATE PROCEDURE [dbo].[sp_Fees_Update]
    @Fees_Id INT,
    @FeeType INT,
    @Amount DECIMAL(10, 2),
    @Currency NVARCHAR(3),
    @Description NVARCHAR(MAX),
    @DueDate DATETIME,
    @IsActive BIT,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE [dbo].[Fees]
        SET 
            [FeeType] = @FeeType,
            [Amount] = @Amount,
            [Currency] = @Currency,
            [Description] = @Description,
            [DueDate] = @DueDate,
            [IsActive] = @IsActive,
            [Modified_By] = @Modified_By,
            [Modified_Date] = GETDATE()
        WHERE [Fees_Id] = @Fees_Id AND [IsDeleted] = 0;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;