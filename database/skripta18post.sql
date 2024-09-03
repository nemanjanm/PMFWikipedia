IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'Post'))
BEGIN
	CREATE TABLE [dbo].[Post](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[Title] [varchar](50) NOT NULL,
		[Content] [varchar](MAX) NOT NULL,
		[Author] [bigint] FOREIGN KEY REFERENCES [dbo].[User](Id),
		[LastEditedBy] [bigint] FOREIGN KEY REFERENCES [dbo].[User](Id),
		[Subject] [bigint] FOREIGN KEY REFERENCES [dbo].[Subject](Id),
		[IsDeleted] [bit] NOT NULL DEFAULT 0,
		[DateCreated] [datetime] NOT NULL,
		[DateModified] [datetime] NOT NULL,
		[LastModifiedBy] [bigint] NOT NULL,
		CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
		(
			   [Id] ASC
		) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	);
END