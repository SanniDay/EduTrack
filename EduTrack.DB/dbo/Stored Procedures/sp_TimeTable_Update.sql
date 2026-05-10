

/* =========================================================
   PROCEDURE : UPDATE
========================================================= */

CREATE PROCEDURE [dbo].[sp_TimeTable_Update]

    @TimeTable_Id INT,
    @Class_Id INT,
    @ClassSubject_Id INT,
    @Teacher_Id INT,
    @Day_Name VARCHAR(20),
    @Period_No INT,
    @Start_Time TIME,
    @End_Time TIME,
    @Room_No NVARCHAR(50),
    @IsActive BIT,
    @Modified_By NVARCHAR(100)

AS
BEGIN

    SET NOCOUNT ON

    /* ==========================================
       DUPLICATE CHECK EXCEPT CURRENT ROW
    ========================================== */

    IF EXISTS
    (
        SELECT 1
        FROM TimeTable
        WHERE
            Class_Id = @Class_Id
            AND Day_Name = @Day_Name
            AND Period_No = @Period_No
            AND TimeTable_Id != @TimeTable_Id
            AND IsDeleted = 0
    )
    BEGIN
        RAISERROR('Timetable already exists for this class and period',16,1)
        RETURN
    END


    UPDATE TimeTable

    SET

        Class_Id = @Class_Id,

        ClassSubject_Id = @ClassSubject_Id,

        Teacher_Id = @Teacher_Id,

        Day_Name = @Day_Name,

        Period_No = @Period_No,

        Start_Time = @Start_Time,

        End_Time = @End_Time,

        Room_No = @Room_No,

        IsActive = @IsActive,

        Modified_By = @Modified_By,

        Modified_Date = GETDATE()

    WHERE

        TimeTable_Id = @TimeTable_Id

        AND IsDeleted = 0

END