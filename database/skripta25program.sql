IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'Program'))
BEGIN
	CREATE TABLE [dbo].[Program](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[Name][varchar](max) NOT NULL,
		[IsDeleted] [bit] NOT NULL DEFAULT 0,
		[DateCreated] [datetime] NOT NULL,
		[DateModified] [datetime] NOT NULL,
		[LastModifiedBy] [bigint] NOT NULL,
		CONSTRAINT [PK_Program] PRIMARY KEY CLUSTERED 
		(
			   [Id] ASC
		) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	);
END