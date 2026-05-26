USE master;
GO

CREATE LOGIN devicemanager_user
WITH PASSWORD = 'Linh@3181992';
GO

#tạo user

USE DeviceManagerDb;
GO

CREATE USER devicemanager_user FOR LOGIN devicemanager_user;
GO

ALTER ROLE db_datareader ADD MEMBER devicemanager_user;
ALTER ROLE db_datawriter ADD MEMBER devicemanager_user;
ALTER ROLE db_ddladmin ADD MEMBER devicemanager_user;
GO