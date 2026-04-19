CREATE PROCEDURE sp_StudentClass_Update
(
    @Student_Class_Id INT,
    @Student_Id INT,
    @Class_Id INT,
    @Modified_By VARCHAR(50)
)
AS
BEGIN

UPDATE StudentClass
SET
    Student_Id = @Student_Id,
    Class_Id = @Class_Id,
    Modified_By = @Modified_By,
    Modified_Date = GETDATE()
WHERE Student_Class_Id = @Student_Class_Id

END