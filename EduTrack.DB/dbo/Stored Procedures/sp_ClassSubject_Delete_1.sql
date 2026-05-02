CREATE PROCEDURE [dbo].[sp_ClassSubject_Delete]
    @ClassSubject_Id INT
AS
BEGIN
    UPDATE ClassSubject
    SET IsDeleted = 1, IsActive = 0, Modified_Date = GETDATE()
    WHERE ClassSubject_Id = @ClassSubject_Id;
END