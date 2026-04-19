CREATE PROCEDURE sp_ClassSubject_Update
    @ClassSubject_Id INT,
    @Class_Id INT,
    @Subject_Id INT,
    @IsCore BIT,
    @Modified_By NVARCHAR(100)
AS
UPDATE ClassSubject
SET Class_Id = @Class_Id,
    Subject_Id = @Subject_Id,
    IsCore = @IsCore,
    Modified_By = @Modified_By,
    Modified_Date = GETDATE()
WHERE ClassSubject_Id = @ClassSubject_Id
