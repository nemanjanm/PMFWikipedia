IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'User'))
BEGIN
	IF(NOT EXISTS (select 1 from sys.columns Where Name='PhotoPath' AND Object_ID = Object_ID('dbo.User')))
	BEGIN
		ALTER TABLE [dbo].[User]
		ADD [PhotoPath] varchar(50) NULL
	END
END