CREATE PROCEDURE sp_Teacher_GetAll
AS
BEGIN
    SELECT 
        T.*,
        U.Phone_Number AS Phone_No
    FROM Teachers T
    JOIN Users U ON T.User_Id = U.User_Id
    WHERE T.isDeleted = 0
END
