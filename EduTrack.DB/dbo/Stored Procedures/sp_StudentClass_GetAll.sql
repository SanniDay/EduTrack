
-- =====================================================
-- STUDENTCLASS STORED PROCEDURES - UPDATED
-- =====================================================

CREATE PROCEDURE [dbo].[sp_StudentClass_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT sc.*, s.FullName AS StudentName, c.ClassName
    FROM StudentClass sc
    JOIN Students s ON s.Student_Id = sc.Student_Id
    JOIN Classes c ON c.Class_Id = sc.Class_Id
    WHERE sc.isDeleted = 0 AND s.isDeleted=0 AND c.IsDeleted = 0;
END;