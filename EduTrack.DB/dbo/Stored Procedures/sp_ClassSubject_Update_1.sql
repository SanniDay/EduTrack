CREATE PROCEDURE [dbo].[sp_ClassSubject_Update]
    @ClassSubject_Id INT,
    @Class_Id INT,
    @Subject_Id INT,
    @IsCore BIT,
    @IsActive BIT,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    UPDATE ClassSubject
    SET Class_Id = @Class_Id,
        Subject_Id = @Subject_Id,
        IsCore = @IsCore,
        IsActive = @IsActive,
        Modified_By = @Modified_By,
        Modified_Date = GETDATE()
    WHERE ClassSubject_Id = @ClassSubject_Id AND IsDeleted = 0;
END