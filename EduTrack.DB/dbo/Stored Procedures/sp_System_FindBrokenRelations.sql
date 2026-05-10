/* =========================================================
   FULL UPDATED PROCEDURE
   SYSTEM BROKEN RELATIONS / UNSTRUCTURED DATA CHECK

   PROCEDURE:
   sp_System_FindBrokenRelations
========================================================= */

CREATE   PROCEDURE [dbo].[sp_System_FindBrokenRelations]
AS
BEGIN

    SET NOCOUNT ON


    /* =====================================================
       ATTENDANCE -> MISSING STUDENTCLASS
    ===================================================== */

    SELECT
        'Attendance -> Missing StudentClass' AS IssueType,
        a.Attendance_Id AS RecordId,
        a.Student_Class_Id AS MissingReferenceId,
        'StudentClass Record Missing' AS Problem,
        'High' AS Severity

    FROM Attendance a

    LEFT JOIN StudentClass sc
        ON sc.Student_Class_Id = a.Student_Class_Id

    WHERE sc.Student_Class_Id IS NULL



    UNION ALL



    /* =====================================================
       ATTENDANCE -> DELETED STUDENT
    ===================================================== */

    SELECT
        'Attendance -> Deleted Student',
        a.Attendance_Id,
        s.Student_Id,
        'Student Is Deleted',
        'High'

    FROM Attendance a

    LEFT JOIN StudentClass sc
        ON sc.Student_Class_Id = a.Student_Class_Id

    LEFT JOIN Students s
        ON s.Student_Id = sc.Student_Id

    WHERE s.isDeleted = 1



    UNION ALL



    /* =====================================================
       STUDENTCLASS -> DELETED STUDENT
    ===================================================== */

    SELECT
        'StudentClass -> Deleted Student',
        sc.Student_Class_Id,
        s.Student_Id,
        'Student Is Deleted',
        'Medium'

    FROM StudentClass sc

    LEFT JOIN Students s
        ON s.Student_Id = sc.Student_Id

    WHERE s.isDeleted = 1



    UNION ALL



    /* =====================================================
       STUDENTCLASS -> DELETED CLASS
    ===================================================== */

    SELECT
        'StudentClass -> Deleted Class',
        sc.Student_Class_Id,
        c.Class_Id,
        'Class Is Deleted',
        'Medium'

    FROM StudentClass sc

    LEFT JOIN Classes c
        ON c.Class_Id = sc.Class_Id

    WHERE c.IsDeleted = 1



    UNION ALL



    /* =====================================================
       TEACHERCLASS -> DELETED TEACHER
    ===================================================== */

    SELECT
        'TeacherClass -> Deleted Teacher',
        tc.Teacher_Class_Id,
        t.Teacher_Id,
        'Teacher Is Deleted',
        'Medium'

    FROM TeacherClass tc

    LEFT JOIN Teachers t
        ON t.Teacher_Id = tc.Teacher_Id

    WHERE t.isDeleted = 1



    UNION ALL



    /* =====================================================
       TEACHERCLASS -> DELETED CLASS
    ===================================================== */

    SELECT
        'TeacherClass -> Deleted Class',
        tc.Teacher_Class_Id,
        c.Class_Id,
        'Class Is Deleted',
        'Medium'

    FROM TeacherClass tc

    LEFT JOIN Classes c
        ON c.Class_Id = tc.Class_Id

    WHERE c.IsDeleted = 1



    UNION ALL



    /* =====================================================
       CLASSSUBJECT -> DELETED SUBJECT
    ===================================================== */

    SELECT
        'ClassSubject -> Deleted Subject',
        cs.ClassSubject_Id,
        s.Subject_Id,
        'Subject Is Deleted',
        'Medium'

    FROM ClassSubject cs

    LEFT JOIN Subjects s
        ON s.Subject_Id = cs.Subject_Id

    WHERE s.IsDeleted = 1



    UNION ALL



    /* =====================================================
       CLASSSUBJECT -> DELETED CLASS
    ===================================================== */

    SELECT
        'ClassSubject -> Deleted Class',
        cs.ClassSubject_Id,
        c.Class_Id,
        'Class Is Deleted',
        'Medium'

    FROM ClassSubject cs

    LEFT JOIN Classes c
        ON c.Class_Id = cs.Class_Id

    WHERE c.IsDeleted = 1



    UNION ALL



    /* =====================================================
       TIMETABLE -> DELETED TEACHER
    ===================================================== */

    SELECT
        'TimeTable -> Deleted Teacher',
        tt.TimeTable_Id,
        t.Teacher_Id,
        'Teacher Is Deleted',
        'Low'

    FROM TimeTable tt

    LEFT JOIN Teachers t
        ON t.Teacher_Id = tt.Teacher_Id

    WHERE t.isDeleted = 1



    UNION ALL



    /* =====================================================
       TIMETABLE -> DELETED CLASS
    ===================================================== */

    SELECT
        'TimeTable -> Deleted Class',
        tt.TimeTable_Id,
        c.Class_Id,
        'Class Is Deleted',
        'Low'

    FROM TimeTable tt

    LEFT JOIN Classes c
        ON c.Class_Id = tt.Class_Id

    WHERE c.IsDeleted = 1



    UNION ALL



    /* =====================================================
       USER -> DELETED ROLE
    ===================================================== */

    SELECT
        'User -> Deleted Role',
        u.User_Id,
        r.Role_Id,
        'Role Is Deleted',
        'Low'

    FROM Users u

    LEFT JOIN Roles r
        ON r.Role_Id = u.Role_Id

    WHERE r.isDeleted = 1



    UNION ALL



    /* =====================================================
       CLASS -> NO SUBJECT MAPPING
    ===================================================== */

    SELECT
        'Class -> No Subject Mapping',
        c.Class_Id,
        NULL,
        'Class Has No Subject Assigned',
        'Low'

    FROM Classes c

    LEFT JOIN ClassSubject cs
        ON cs.Class_Id = c.Class_Id
        AND cs.IsDeleted = 0

    WHERE
        c.IsDeleted = 0
        AND cs.ClassSubject_Id IS NULL



    UNION ALL



    /* =====================================================
       SUBJECT -> NOT ASSIGNED TO CLASS
    ===================================================== */

    SELECT
        'Subject -> Not Assigned To Any Class',
        s.Subject_Id,
        NULL,
        'Subject Not Used In Any Class',
        'Low'

    FROM Subjects s

    LEFT JOIN ClassSubject cs
        ON cs.Subject_Id = s.Subject_Id
        AND cs.IsDeleted = 0

    WHERE
        s.IsDeleted = 0
        AND cs.ClassSubject_Id IS NULL



    UNION ALL



    /* =====================================================
       CLASSSUBJECT -> MISSING CLASS
    ===================================================== */

    SELECT
        'ClassSubject -> Missing Class',
        cs.ClassSubject_Id,
        cs.Class_Id,
        'Referenced Class Missing',
        'High'

    FROM ClassSubject cs

    LEFT JOIN Classes c
        ON c.Class_Id = cs.Class_Id

    WHERE c.Class_Id IS NULL



    UNION ALL



    /* =====================================================
       CLASSSUBJECT -> MISSING SUBJECT
    ===================================================== */

    SELECT
        'ClassSubject -> Missing Subject',
        cs.ClassSubject_Id,
        cs.Subject_Id,
        'Referenced Subject Missing',
        'High'

    FROM ClassSubject cs

    LEFT JOIN Subjects s
        ON s.Subject_Id = cs.Subject_Id

    WHERE s.Subject_Id IS NULL



    UNION ALL



    /* =====================================================
       TEACHERCLASS -> MISSING CLASSSUBJECT
    ===================================================== */

    SELECT
        'TeacherClass -> Missing ClassSubject',
        tc.Teacher_Class_Id,
        tc.ClassSubject_Id,
        'ClassSubject Missing',
        'High'

    FROM TeacherClass tc

    LEFT JOIN ClassSubject cs
        ON cs.ClassSubject_Id = tc.ClassSubject_Id

    WHERE
        tc.ClassSubject_Id IS NOT NULL
        AND cs.ClassSubject_Id IS NULL



    UNION ALL



    /* =====================================================
       TIMETABLE -> MISSING CLASSSUBJECT
    ===================================================== */

    SELECT
        'TimeTable -> Missing ClassSubject',
        tt.TimeTable_Id,
        tt.ClassSubject_Id,
        'ClassSubject Missing',
        'High'

    FROM TimeTable tt

    LEFT JOIN ClassSubject cs
        ON cs.ClassSubject_Id = tt.ClassSubject_Id

    WHERE cs.ClassSubject_Id IS NULL



    UNION ALL



    /* =====================================================
       TIMETABLE -> DELETED SUBJECT
    ===================================================== */

    SELECT
        'TimeTable -> Deleted Subject',
        tt.TimeTable_Id,
        s.Subject_Id,
        'Subject Is Deleted',
        'Medium'

    FROM TimeTable tt

    LEFT JOIN ClassSubject cs
        ON cs.ClassSubject_Id = tt.ClassSubject_Id

    LEFT JOIN Subjects s
        ON s.Subject_Id = cs.Subject_Id

    WHERE s.IsDeleted = 1



    UNION ALL



    /* =====================================================
       STUDENTFEES -> DELETED STUDENT
    ===================================================== */

    SELECT
        'StudentFees -> Deleted Student',
        sf.StudentFees_Id,
        s.Student_Id,
        'Student Is Deleted',
        'Medium'

    FROM StudentFees sf

    LEFT JOIN Students s
        ON s.Student_Id = sf.Student_Id

    WHERE s.isDeleted = 1



    UNION ALL



    /* =====================================================
       STUDENTFEES -> DELETED FEES
    ===================================================== */

    SELECT
        'StudentFees -> Deleted Fees',
        sf.StudentFees_Id,
        f.Fees_Id,
        'Fees Record Deleted',
        'Medium'

    FROM StudentFees sf

    LEFT JOIN Fees f
        ON f.Fees_Id = sf.Fees_Id

    WHERE f.IsDeleted = 1



    UNION ALL



    /* =====================================================
       USERS -> MISSING ROLE
    ===================================================== */

    SELECT
        'User -> Missing Role',
        u.User_Id,
        u.Role_Id,
        'Referenced Role Missing',
        'High'

    FROM Users u

    LEFT JOIN Roles r
        ON r.Role_Id = u.Role_Id

    WHERE
        u.Role_Id IS NOT NULL
        AND r.Role_Id IS NULL



    UNION ALL



    /* =====================================================
       STUDENTS -> MISSING USER
    ===================================================== */

    SELECT
        'Student -> Missing User',
        s.Student_Id,
        s.User_Id,
        'Referenced User Missing',
        'High'

    FROM Students s

    LEFT JOIN Users u
        ON u.User_Id = s.User_Id

    WHERE u.User_Id IS NULL



    UNION ALL



    /* =====================================================
       TEACHERS -> MISSING USER
    ===================================================== */

    SELECT
        'Teacher -> Missing User',
        t.Teacher_Id,
        t.User_Id,
        'Referenced User Missing',
        'High'

    FROM Teachers t

    LEFT JOIN Users u
        ON u.User_Id = t.User_Id

    WHERE u.User_Id IS NULL



    ORDER BY Severity DESC

END