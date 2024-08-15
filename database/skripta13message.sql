IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'Message'))
BEGIN
	IF(NOT EXISTS (select 1 from sys.columns Where Name='IsDeleted' AND Object_ID = Object_ID('dbo.Message'))
	AND NOT EXISTS (select 1 from sys.columns Where Name='DateCreated' AND Object_ID = Object_ID('dbo.Message'))
	AND NOT EXISTS (select 1 from sys.columns Where Name='DateModified' AND Object_ID = Object_ID('dbo.Message'))
	AND NOT EXISTS (select 1 from sys.columns Where Name='LastModifiedBy' AND Object_ID = Object_ID('dbo.Message')))
	BEGIN
		ALTER TABLE [dbo].[Message]
		ADD [IsDeleted] [bit] NOT NULL DEFAULT 0,
		[DateCreated] [datetime] NOT NULL,
		[DateModified] [datetime] NOT NULL,
		[LastModifiedBy] [bigint] NOT NULL
	END
END
