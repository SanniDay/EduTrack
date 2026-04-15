CREATE PROCEDURE sp_Class_Delete
(
    @Class_Id INT
)
AS
BEGIN

UPDATE Class
SET
    isDeleted = 1,
    isActive = 0
WHERE Class_Id = @Class_Id

END