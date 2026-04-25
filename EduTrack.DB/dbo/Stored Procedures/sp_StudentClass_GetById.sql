
CREATE PROCEDURE [dbo].[sp_StudentClass_GetById]
    @Student_Class_Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT sc.*, s.FullName AS StudentName, c.ClassName
    FROM StudentClass sc
    JOIN Students s ON s.Student_Id = sc.Student_Id
    JOIN Classes c ON c.Class_Id = sc.Class_Id
    WHERE sc.Student_Class_Id = @Student_Class_Id AND sc.isDeleted = 0;
END;