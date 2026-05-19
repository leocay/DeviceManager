DROP TABLE IF EXISTS #EmployeesCsv;

CREATE TABLE #EmployeesCsv
(
    EmployeeId INT,
    EmployeeCode NVARCHAR(50),
    FullName NVARCHAR(255),
    Department NVARCHAR(100),
    Position NVARCHAR(100),
    Email NVARCHAR(255),
    PhoneNumber NVARCHAR(50),
    CreateAt NVARCHAR(50)
);

BULK INSERT #EmployeesCsv
FROM '/var/opt/mssql/import/userCAET_utf16.csv'
WITH
(
    DATAFILETYPE = 'widechar',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '0x0a00',
    TABLOCK
);

DELETE FROM [DeviceManagerDb].[dbo].[Employees];

INSERT INTO [DeviceManagerDb].[dbo].[Employees]
(
    [EmployeeCode],
    [FullName],
    [Department],
    [Position],
    [Email],
    [PhoneNumber],
    [CreatedAt]
)
SELECT
    EmployeeCode,
    FullName,
    Department,
    Position,
    NULLIF(Email, ''),
    PhoneNumber,
    GETDATE()
FROM #EmployeesCsv;

DROP TABLE IF EXISTS #EmployeesCsv;