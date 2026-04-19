CREATE PROCEDURE sp_StudentClass_Create
(
    @Student_Id INT,
    @Class_Id INT,
    @Created_By VARCHAR(50)
)
AS
BEGIN

INSERT INTO StudentClass
(
    Student_Id,
    Class_Id,
    Created_By,
    Created_Date,
    Modified_By,
    Modified_Date,
    isActive,
    isDeleted
)
VALUES
(
    @Student_Id,
    @Class_Id,
    @Created_By,
    GETDATE(),
    @Created_By,
    GETDATE(),
    1,
    0
)

END