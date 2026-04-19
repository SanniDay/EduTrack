CREATE PROCEDURE sp_ClassSubject_Delete
    @ClassSubject_Id INT
AS
UPDATE ClassSubject
SET IsDeleted = 1,
    Modified_Date = GETDATE()
WHERE ClassSubject_Id = @ClassSubject_Id
