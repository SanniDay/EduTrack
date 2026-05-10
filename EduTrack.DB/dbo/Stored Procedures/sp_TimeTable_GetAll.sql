

/* =========================================================
   PROCEDURE : GET ALL
========================================================= */

CREATE PROCEDURE [dbo].[sp_TimeTable_GetAll]

AS
BEGIN

    SET NOCOUNT ON

    SELECT

        tt.TimeTable_Id,
        tt.Day_Name,
        tt.Period_No,
        tt.Start_Time,
        tt.End_Time,
        tt.Room_No,

        c.Class_Id,
        c.ClassName,
        c.Section,

        s.Subject_Id,
        s.Subject_Name,

        t.Teacher_Id,
        t.FullName AS TeacherName,

        tt.IsActive,
        tt.Created_By,
        tt.Created_Date,
        tt.Modified_By,
        tt.Modified_Date

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
        tt.IsDeleted = 0

    ORDER BY
        tt.Day_Name,
        tt.Period_No

END