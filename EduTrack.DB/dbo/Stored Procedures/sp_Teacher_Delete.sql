
CREATE PROCEDURE sp_Teacher_Delete
(
    @Teacher_Id INT
)
AS
BEGIN
    UPDATE Teachers
    SET
        isDeleted = 1,
        isActive = 0,
        Modified_Date = GETDATE()
    WHERE Teacher_Id = @Teacher_Id
END