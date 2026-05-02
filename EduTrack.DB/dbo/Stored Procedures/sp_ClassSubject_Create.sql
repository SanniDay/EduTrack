CREATE PROCEDURE [dbo].[sp_ClassSubject_Create]
    @Class_Id INT,
    @Subject_Id INT,
    @IsCore BIT = 1,
    @IsActive BIT = 1,
    @Created_By NVARCHAR(100)
AS
BEGIN
    INSERT INTO ClassSubject(Class_Id, Subject_Id, IsCore, IsActive, IsDeleted, Created_By, Created_Date, Modified_By, Modified_Date)
    VALUES(@Class_Id, @Subject_Id, @IsCore, @IsActive, 0, @Created_By, GETDATE(), @Created_By, GETDATE());
END