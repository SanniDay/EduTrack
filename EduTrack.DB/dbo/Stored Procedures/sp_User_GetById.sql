
CREATE PROCEDURE [dbo].[sp_User_GetById]
	@User_Id INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	
			[User_Id],[User_Name],[PasswordHash],[Email],[Phone_Number],[Address],U.[Role_Id], R.[Role_Name], U.[Created_By],
			U.[Created_Date],U.[Modified_By],U.[Modified_Date],U.[isActive],U.[isDeleted] 
	FROM Users U
	LEFT JOIN Roles R ON R.Role_Id=U.Role_Id
	WHERE U.isDeleted=0 AND [User_Id]=@User_Id
END
