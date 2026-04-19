CREATE PROCEDURE sp_Student_GetAll
AS
BEGIN
    SELECT 
        S.*,
        U.Phone_Number AS Phone_No,
        U.Address AS Address
    FROM Students S
    JOIN Users U ON S.User_Id = U.User_Id
    WHERE S.isDeleted = 0
END
