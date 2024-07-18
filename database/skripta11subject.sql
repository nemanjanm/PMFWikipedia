IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'Subject'))
BEGIN
	IF(NOT EXISTS (select 1 from sys.columns Where Name='Year' AND Object_ID = Object_ID('dbo.Subject'))
	AND NOT EXISTS (select 1 from sys.columns Where Name='Semester' AND Object_ID = Object_ID('dbo.Subject')))
	BEGIN
	
		ALTER TABLE [dbo].[Subject]
		ADD [Year] int NOT NULL
	END
	BEGIN
		ALTER TABLE [dbo].[Subject]
		ADD [Semester] int NOT NULL
	END
END

