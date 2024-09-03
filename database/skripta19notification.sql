IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'Notification'))
BEGIN
	CREATE TABLE [dbo].[Notification](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[Author] [bigint] FOREIGN KEY REFERENCES [dbo].[User](Id) NOT NULL,
		[Subject] [bigint] FOREIGN KEY REFERENCES [dbo].[Subject](Id) NOT NULL,
		[Post] [bigint] FOREIGN KEY REFERENCES [dbo].[Post](Id) NOT NULL,
		[Receiver][bigint] FOREIGN KEY REFERENCES [dbo].[User](Id) NOT NULL,
		[IsRead] [bit] NOT NULL DEFAULT 0,
		[IsDeleted] [bit] NOT NULL DEFAULT 0,
		[DateCreated] [datetime] NOT NULL,
		[DateModified] [datetime] NOT NULL,
		[LastModifiedBy] [bigint] NOT NULL,
		CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
		(
			   [Id] ASC
		) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	);
END