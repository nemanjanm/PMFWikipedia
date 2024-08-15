IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'Message'))
BEGIN
	IF(NOT EXISTS (select 1 from sys.columns Where Name='ChatId' AND Object_ID = Object_ID('dbo.Message')))
	BEGIN
		ALTER TABLE [dbo].[Message]
		ADD [ChatId] [bigint] FOREIGN KEY REFERENCES [dbo].[Chat](Id)
	END
END
