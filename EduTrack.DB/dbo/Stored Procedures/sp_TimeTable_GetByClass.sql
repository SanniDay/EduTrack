

/* =========================================================
   PROCEDURE : GET TIMETABLE BY CLASS
========================================================= */

CREATE PROCEDURE [dbo].[sp_TimeTable_GetByClass]

    @Class_Id INT

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

        s.Subject_Name,

        t.FullName AS TeacherName

    FROM TimeTable tt

    INNER JOIN ClassSubject cs
        ON cs.ClassSubject_Id = tt.ClassSubject_Id

    INNER JOIN Subjects s
        ON s.Subject_Id = cs.Subject_Id

    INNER JOIN Teachers t
        ON t.Teacher_Id = tt.Teacher_Id

    WHERE
        tt.Class_Id = @Class_Id
        AND tt.IsDeleted = 0

    ORDER BY
        tt.Day_Name,
        tt.Period_No

END