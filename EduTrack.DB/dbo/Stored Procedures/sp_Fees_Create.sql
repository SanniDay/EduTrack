
CREATE PROCEDURE [dbo].[sp_Fees_Create]
    @Class_Id INT,
    @FeeType INT,
    @Amount DECIMAL(10, 2),
    @Currency NVARCHAR(3),
    @Description NVARCHAR(MAX),
    @DueDate DATETIME,
    @Created_By NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO [dbo].[Fees] 
            ([Class_Id], [FeeType], [Amount], [Currency], [Description], [DueDate], [IsActive], [IsDeleted], [Created_By], [Created_Date])
        VALUES 
            (@Class_Id, @FeeType, @Amount, @Currency, @Description, @DueDate, 1, 0, @Created_By, GETDATE());
        
        SELECT SCOPE_IDENTITY() AS [Fees_Id];
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;