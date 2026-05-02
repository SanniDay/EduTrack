
CREATE PROCEDURE [dbo].[sp_StudentFees_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        sf.[StudentFees_Id],
        sf.[Student_Id],
        s.FullName AS StudentName,
        sf.[Fees_Id],
        f.[Class_Id],
        c.ClassName,
        f.FeeType,
        sf.[Amount],
        sf.[PaymentStatus],
        sf.[DueDate],
        sf.[PaidDate],
        sf.[PaymentMethod],
        sf.[Receipt_No],
        sf.[DiscountAmount],
        sf.[FineAmount],
        sf.[TotalAmount],
        sf.[Notes],
        sf.[IsDeleted],
        sf.[Created_By],
        sf.[Created_Date],
        sf.[Modified_By],
        sf.[Modified_Date]
    FROM [dbo].[StudentFees] sf
    JOIN [dbo].[Students] s ON s.[Student_Id] = sf.[Student_Id]
    JOIN [dbo].[Fees] f ON f.[Fees_Id] = sf.[Fees_Id]
    JOIN [dbo].[Classes] c ON c.[Class_Id] = f.[Class_Id]
    WHERE sf.[IsDeleted] = 0
    ORDER BY sf.[DueDate] DESC;
END;