CREATE PROCEDURE sp_TeacherClass_Create
(
    @Teacher_Id INT,
    @Class_Id INT,
    @Subject VARCHAR(100),
    @Created_By VARCHAR(50)
)
AS
BEGIN

INSERT INTO TeacherClass
(
    Teacher_Id,
    Class_Id,
    Subject,
    Created_By,
    Created_Date,
    Modified_By,
    Modified_Date,
    isActive,
    isDeleted
)
VALUES
(
    @Teacher_Id,
    @Class_Id,
    @Subject,
    @Created_By,
    GETDATE(),
    @Created_By,
    GETDATE(),
    1,
    0
)

END