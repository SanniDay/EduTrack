
CREATE PROCEDURE [dbo].[sp_Class_GetAll]
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
        IsActive,
        IsDeleted
    FROM Classes
    WHERE IsDeleted = 0

END