IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'Chat'))
BEGIN
	CREATE TABLE [dbo].[Chat](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[User1] [bigint] FOREIGN KEY REFERENCES [dbo].[User](Id),
		[User2] [bigint] FOREIGN KEY REFERENCES [dbo].[User](Id),
		[IsDeleted] [bit] NOT NULL DEFAULT 0,
		[DateCreated] [datetime] NOT NULL,
		[DateModified] [datetime] NOT NULL,
		[LastModifiedBy] [bigint] NOT NULL
		CONSTRAINT [PK_Chat] PRIMARY KEY CLUSTERED 
		(
			   [Id] ASC
		) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	);
END