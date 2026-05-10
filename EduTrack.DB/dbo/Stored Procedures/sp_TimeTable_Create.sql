

/* =========================================================
   PROCEDURE : CREATE
========================================================= */

CREATE PROCEDURE [dbo].[sp_TimeTable_Create]

    @Class_Id INT,
    @ClassSubject_Id INT,
    @Teacher_Id INT,
    @Day_Name VARCHAR(20),
    @Period_No INT,
    @Start_Time TIME,
    @End_Time TIME,
    @Room_No NVARCHAR(50),
    @Created_By NVARCHAR(100)

AS
BEGIN

    SET NOCOUNT ON

    /* ==========================================
       OPTIONAL DUPLICATE CHECK
       ONLY ACTIVE RECORDS
    ========================================== */

    IF EXISTS
    (
        SELECT 1
        FROM TimeTable
        WHERE
            Class_Id = @Class_Id
            AND Day_Name = @Day_Name
            AND Period_No = @Period_No
            AND IsDeleted = 0
    )
    BEGIN
        RAISERROR('Timetable already exists for this class and period',16,1)
        RETURN
    END


    INSERT INTO TimeTable
    (
        Class_Id,
        ClassSubject_Id,
        Teacher_Id,
        Day_Name,
        Period_No,
        Start_Time,
        End_Time,
        Room_No,
        IsActive,
        IsDeleted,
        Created_By,
        Created_Date,
        Modified_By,
        Modified_Date
    )

    VALUES
    (
        @Class_Id,
        @ClassSubject_Id,
        @Teacher_Id,
        @Day_Name,
        @Period_No,
        @Start_Time,
        @End_Time,
        @Room_No,
        1,
        0,
        @Created_By,
        GETDATE(),
        @Created_By,
        GETDATE()
    )

END