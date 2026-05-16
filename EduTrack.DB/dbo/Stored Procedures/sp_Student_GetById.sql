

/* =========================================
   ALTER: sp_Student_GetById
   ADD Class_Id
========================================= */

CREATE PROCEDURE [dbo].[sp_Student_GetById]
(
    @Student_Id INT
)
AS
BEGIN
    SELECT 
        S.*,
        U.Phone_Number AS Phone_No,
        U.Address AS Address,
        SC.Class_Id
    FROM Students S
    JOIN Users U 
        ON S.User_Id = U.User_Id
    LEFT JOIN StudentClass SC
        ON S.Student_Id = SC.Student_Id
        AND SC.isDeleted = 0
    WHERE 
        S.Student_Id = @Student_Id 
        AND S.isDeleted = 0
        AND U.isDeleted = 0
END