IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'Comment'))
BEGIN
	CREATE TABLE [dbo].[Comment](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[Content] [varchar](max) NOT NULL,
		[UserId] [bigint]  NOT NULL,
		[PostId] [bigint]  NOT NULL,
		[IsDeleted] [bit] NOT NULL DEFAULT 0,
		[DateCreated] [datetime] NOT NULL,
		[DateModified] [datetime] NOT NULL,
		[LastModifiedBy] [bigint] NOT NULL,
		CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
		(
			   [Id] ASC
		) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
		CONSTRAINT [FK_Comment_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User](Id),
		CONSTRAINT [FK_Comment_PostId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Post](Id),
	);
END