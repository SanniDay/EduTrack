

/* ===============================
   FIX: STUDENTCLASS UPDATE
================================ */

CREATE PROCEDURE [dbo].[sp_StudentClass_Update]
(
    @Student_Class_Id INT,
    @Student_Id INT,
    @Class_Id INT,
    @isActive BIT,
    @Modified_By VARCHAR(50)
)
AS
BEGIN
    UPDATE StudentClass
    SET Student_Id = @Student_Id,
        Class_Id = @Class_Id,
        isActive = @isActive,
        Modified_By = @Modified_By,
        Modified_Date = GETDATE()
    WHERE 
        Student_Class_Id = @Student_Class_Id
        AND isDeleted = 0   -- FIX ADDED
END