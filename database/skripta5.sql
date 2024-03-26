IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'User'))
BEGIN
	IF(NOT EXISTS (select 1 from sys.columns Where Name='RegisterToken' AND Object_ID = Object_ID('dbo.User'))
	AND NOT EXISTS (select 1 from sys.columns Where Name='RegisterTokenExpirationTime' AND Object_ID = Object_ID('dbo.User')))
	BEGIN
		ALTER TABLE [dbo].[User]
		ADD [RegisterToken] varchar(50) NOT NULL
	END
	BEGIN
		ALTER TABLE [dbo].[User]
		ADD [RegisterTokenExpirationTime] datetime NOT NULL
	END
END

