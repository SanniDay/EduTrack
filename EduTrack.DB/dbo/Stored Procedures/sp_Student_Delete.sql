
CREATE PROCEDURE sp_Student_Delete
(
    @Student_Id INT
)
AS
BEGIN
    UPDATE Students
    SET
        isDeleted = 1,
        isActive = 0,
        Modified_Date = GETDATE()
    WHERE Student_Id = @Student_Id
END