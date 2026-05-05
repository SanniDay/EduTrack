

CREATE PROCEDURE [dbo].[sp_Teacher_Update]
(
    @Teacher_Id INT,
    @User_Id INT,
    @FullName VARCHAR(100),
    @Created_By VARCHAR(50),
    @Modified_By VARCHAR(50),
    @IsActive BIT,
    @IsDeleted BIT
)
AS
BEGIN
    UPDATE Teachers
    SET
        User_Id = @User_Id,
        FullName = @FullName,
        Created_By = @Created_By,
        Modified_By = @Modified_By,
        IsActive = @IsActive,
        IsDeleted = @IsDeleted,
        Modified_Date = GETDATE()
    WHERE 
        Teacher_Id = @Teacher_Id
        AND isDeleted = 0   -- FIX ADDED
END