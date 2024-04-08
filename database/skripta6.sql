IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'User'))
BEGIN
	IF(NOT EXISTS (select 1 from sys.columns Where Name='ResetToken' AND Object_ID = Object_ID('dbo.User'))
	AND NOT EXISTS (select 1 from sys.columns Where Name='ResetTokenExpirationTime' AND Object_ID = Object_ID('dbo.User')))
	BEGIN
		ALTER TABLE [dbo].[User]
		ADD [ResetToken] varchar(50) NULL
	END
	BEGIN
		ALTER TABLE [dbo].[User]
		ADD [ResetTokenExpirationTime] datetime NULL
	END
END

