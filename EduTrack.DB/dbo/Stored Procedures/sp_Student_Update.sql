

CREATE PROCEDURE [dbo].[sp_Student_Update]
(
    @Student_Id INT,
    @FullName VARCHAR(100),
    @DOB DATE,
    @Gender VARCHAR(10),
    @Modified_By VARCHAR(50),
    @isActive BIT
)
AS
BEGIN
    UPDATE Students
    SET
        FullName = @FullName,
        DOB = @DOB,
        Gender = @Gender,
        Modified_By = @Modified_By,
        isActive = @isActive,
        Modified_Date = GETDATE()
    WHERE 
        Student_Id = @Student_Id
        AND isDeleted = 0   -- FIX ADDED
END