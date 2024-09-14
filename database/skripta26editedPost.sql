IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'EditedPost'))
BEGIN
	CREATE TABLE [dbo].[EditedPost](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[Title] [varchar](50) NOT NULL,
		[Content] [varchar](MAX) NOT NULL,
		[Time][varchar](50) NOT NULL,
		[AuthorId] [bigint]  NOT NULL,
		[PostId] [bigint] NOT NULL,
		[SubjectId] [bigint] NOT NULL,
		[IsDeleted] [bit] NOT NULL DEFAULT 0,
		[DateCreated] [datetime] NOT NULL,
		[DateModified] [datetime] NOT NULL,
		[LastModifiedBy] [bigint] NOT NULL,
		CONSTRAINT [PK_EditedPost] PRIMARY KEY CLUSTERED 
		(
			   [Id] ASC
		) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
		CONSTRAINT [FK_EditedPost_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[User](Id),
		CONSTRAINT [FK_EditedPost_PostId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Post](Id),
		CONSTRAINT [FK_SubjectId_IspitId] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subject](Id),
	);
END