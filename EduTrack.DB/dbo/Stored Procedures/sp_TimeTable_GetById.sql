

/* =========================================================
   PROCEDURE : GET BY ID
========================================================= */

CREATE PROCEDURE [dbo].[sp_TimeTable_GetById]

    @TimeTable_Id INT

AS
BEGIN

    SET NOCOUNT ON

    SELECT

        tt.*,

        c.ClassName,
        c.Section,

        s.Subject_Name,

        t.FullName AS TeacherName

    FROM TimeTable tt

    INNER JOIN Classes c
        ON c.Class_Id = tt.Class_Id

    INNER JOIN ClassSubject cs
        ON cs.ClassSubject_Id = tt.ClassSubject_Id

    INNER JOIN Subjects s
        ON s.Subject_Id = cs.Subject_Id

    INNER JOIN Teachers t
        ON t.Teacher_Id = tt.Teacher_Id

    WHERE
        tt.TimeTable_Id = @TimeTable_Id
        AND tt.IsDeleted = 0

END