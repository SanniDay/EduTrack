CREATE PROCEDURE sp_StudentClass_GetById
(
    @Student_Class_Id INT
)
AS
BEGIN

SELECT *
FROM StudentClass
WHERE Student_Class_Id = @Student_Class_Id
AND isDeleted = 0

END