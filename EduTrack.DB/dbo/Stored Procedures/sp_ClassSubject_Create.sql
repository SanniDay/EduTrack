
/* =========================================================
   6. CLASSSUBJECT SP
   ========================================================= */
CREATE   PROCEDURE sp_ClassSubject_Create
@Class_Id INT,@Subject_Id INT,@Created_By NVARCHAR(100)
AS
INSERT INTO ClassSubject(Class_Id,Subject_Id,Created_By)
VALUES(@Class_Id,@Subject_Id,@Created_By);