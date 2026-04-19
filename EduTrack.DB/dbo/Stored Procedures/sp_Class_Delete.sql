CREATE PROCEDURE sp_Class_Delete
(
    @Class_Id INT
)
AS
BEGIN

UPDATE Classes
SET
    IsDeleted = 1,
    IsActive = 0
WHERE Class_Id = @Class_Id

END