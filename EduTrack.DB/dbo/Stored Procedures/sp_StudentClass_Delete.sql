CREATE PROCEDURE sp_StudentClass_Delete
(
    @Student_Class_Id INT
)
AS
BEGIN

UPDATE StudentClass
SET
    isDeleted = 1,
    isActive = 0
WHERE Student_Class_Id = @Student_Class_Id

END