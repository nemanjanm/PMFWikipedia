IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'User'))
BEGIN

	IF(NOT EXISTS (select 1 from sys.columns Where Name='Verified' AND Object_ID = Object_ID('dbo.User')))
	BEGIN

		ALTER TABLE [dbo].[User]
		ADD [Verified] bit NOT NULL
	END
END
