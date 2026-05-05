

/* ===============================
   FIX: CLASS
================================ */

CREATE PROCEDURE [dbo].[sp_Class_Update]
(
    @Class_Id INT,
    @ClassName VARCHAR(50),
    @Section VARCHAR(10),
    @Modified_By VARCHAR(50)
)
AS
BEGIN
    UPDATE Classes
    SET
        ClassName = @ClassName,
        Section = @Section,
        Modified_By = @Modified_By,
        Modified_Date = GETDATE()
    WHERE 
        Class_Id = @Class_Id
        AND IsDeleted = 0   -- FIX ADDED
END