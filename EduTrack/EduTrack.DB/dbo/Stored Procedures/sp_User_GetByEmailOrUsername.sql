CREATE PROCEDURE [dbo].[sp_User_GetByEmailOrUsername]
    @EmailOrUsername NVARCHAR(100)
AS
BEGIN
    SELECT 
        User_Id,
        User_Name,
        Email,
        isActive,
        isDeleted
    FROM Users
    WHERE 
        ([User_Name] = @EmailOrUsername OR Email = @EmailOrUsername);
END
