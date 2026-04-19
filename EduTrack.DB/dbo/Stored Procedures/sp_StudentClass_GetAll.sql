CREATE PROCEDURE sp_StudentClass_GetAll
AS
BEGIN

SELECT *
FROM StudentClass
WHERE isDeleted = 0

END