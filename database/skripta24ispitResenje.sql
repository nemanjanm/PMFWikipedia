IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'IspitResenje'))
BEGIN
	CREATE TABLE [dbo].[IspitResenje](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[FilePath] [varchar] (100)  NOT NULL,
		[AuthorId] [bigint]  NOT NULL,
		[IspitId] [bigint]  NOT NULL,
		[SubjectId] [bigint]  NOT NULL,
		[IsDeleted] [bit] NOT NULL DEFAULT 0,
		[DateCreated] [datetime] NOT NULL,
		[DateModified] [datetime] NOT NULL,
		[LastModifiedBy] [bigint] NOT NULL,
		CONSTRAINT [PK_IspitResenje] PRIMARY KEY CLUSTERED 
		(
			   [Id] ASC
		) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
		CONSTRAINT [FK_IspitResenje_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[User](Id),
		CONSTRAINT [FK_IspitResenje_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subject](Id),
		CONSTRAINT [FK_IspitResenje_IspitId] FOREIGN KEY ([IspitId]) REFERENCES [dbo].[Ispit](Id),
	);
END