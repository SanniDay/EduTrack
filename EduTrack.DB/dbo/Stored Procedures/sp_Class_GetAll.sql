CREATE PROCEDURE sp_Class_GetAll
AS
BEGIN

SELECT
    Class_Id,
    ClassName,
    Section,
    Created_By,
    Created_Date,
    Modified_By,
    Modified_Date,
    isActive,
    isDeleted
FROM Class
WHERE isDeleted = 0

END