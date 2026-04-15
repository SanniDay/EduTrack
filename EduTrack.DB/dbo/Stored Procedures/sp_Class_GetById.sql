CREATE PROCEDURE sp_Class_GetById
(
    @Class_Id INT
)
AS
BEGIN

SELECT *
FROM Class
WHERE Class_Id = @Class_Id
AND isDeleted = 0

END