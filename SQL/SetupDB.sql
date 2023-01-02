USE master
GO

--USE master; DROP DATABASE NLog;
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'NLog')
BEGIN
  CREATE DATABASE NLog;
END;
GO

USE NLog;
IF OBJECT_ID('Logs', 'U') IS NULL
BEGIN
	CREATE TABLE Logs(
		Id bigint NOT NULL PRIMARY KEY IDENTITY(1,1),
		CreatedOn nvarchar(max),
		Level nvarchar(10),
		AppUser nvarchar(20),
		Message nvarchar(max),
		StackTrace nvarchar(max),
		Exception nvarchar(max),
		Logger nvarchar(255),
		Url nvarchar(255)
	);
END;
GO

SELECT * FROM Logs ORDER BY Id;