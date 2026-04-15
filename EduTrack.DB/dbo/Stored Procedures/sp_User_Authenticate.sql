CREATE PROCEDURE [dbo].[sp_User_Authenticate]
    @EmailOrUsername NVARCHAR(100),
    @PasswordHash NVARCHAR(256)
AS
BEGIN
    SELECT * 
    FROM Users
    WHERE 
        ([User_Name] = @EmailOrUsername OR Email = @EmailOrUsername)
        AND PasswordHash = @PasswordHash
        AND isDeleted = 0
        AND isActive = 1;  
END
