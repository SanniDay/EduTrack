CREATE PROCEDURE [dbo].[sp_Class_GetById]
(
    @Class_Id INT
)
AS
BEGIN

SELECT 
    Class_Id,
    ClassName,
    Section,
    IsActive,
    IsDeleted,
    Created_By,
    Created_Date,
    Modified_By,
    Modified_Date
FROM Classes
WHERE Class_Id = @Class_Id
AND IsDeleted = 0

END