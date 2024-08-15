IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'Message'))
BEGIN
	IF(EXISTS (select 1 from sys.columns Where Name='ReceiverId' AND Object_ID = Object_ID('dbo.Message')) 
	AND EXISTS (select 1 from sys.columns Where Name='SenderId' AND Object_ID = Object_ID('dbo.Message')))
	BEGIN
		ALTER TABLE [dbo].[Message]
		DROP COLUMN [ReceiverId], [SenderId]
	END
END
