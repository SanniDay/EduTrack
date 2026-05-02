CREATE PROCEDURE [dbo].[sp_StudentClass_Create]
(
    @Student_Id INT,
    @Class_Id INT,
    @isActive BIT = 1,
    @Created_By VARCHAR(50)
)
AS
BEGIN
    INSERT INTO StudentClass(Student_Id, Class_Id, Created_By, Created_Date, Modified_By, Modified_Date, isActive, isDeleted)
    VALUES(@Student_Id, @Class_Id, @Created_By, GETDATE(), @Created_By, GETDATE(), @isActive, 0)
END