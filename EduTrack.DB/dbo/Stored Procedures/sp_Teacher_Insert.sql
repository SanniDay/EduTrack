CREATE PROCEDURE [dbo].[sp_Teacher_Insert]
(
    @User_Id INT,
    @FullName VARCHAR(100),
    @Created_By VARCHAR(50),
    @Modified_By VARCHAR(50),
    @IsActive BIT,
    @IsDeleted BIT
)
AS
BEGIN
    INSERT INTO Teachers (User_Id, FullName, Created_By, Modified_By, IsActive, IsDeleted)
    VALUES (@User_Id, @FullName, @Created_By, @Modified_By, @IsActive, @IsDeleted)
    
    SELECT SCOPE_IDENTITY()
END
