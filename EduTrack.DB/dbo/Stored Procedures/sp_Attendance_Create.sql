
/* =========================================================
   8. ATTENDANCE SP
   ========================================================= */
CREATE   PROCEDURE sp_Attendance_Create
@Student_Class_Id INT,@ClassSubject_Id INT,@Date DATE,@Status VARCHAR(10),@Teacher INT,@Created_By NVARCHAR(100)
AS
INSERT INTO Attendance(Student_Class_Id,ClassSubject_Id,Attendance_Date,Status,Marked_By_Teacher_Id,Created_By)
VALUES(@Student_Class_Id,@ClassSubject_Id,@Date,@Status,@Teacher,@Created_By);