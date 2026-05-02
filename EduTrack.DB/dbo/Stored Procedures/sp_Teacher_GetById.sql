CREATE PROCEDURE [dbo].[sp_Teacher_GetById]
(
    @Teacher_Id INT
)
AS
BEGIN
    SELECT 
        T.*,
        U.Phone_Number AS Phone_No
    FROM Teachers T
    JOIN Users U ON T.User_Id = U.User_Id
    WHERE T.Teacher_Id = @Teacher_Id AND U.isDeleted = 0
END