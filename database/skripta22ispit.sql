IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'Ispit'))
BEGIN
	CREATE TABLE [dbo].[Ispit](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[Title] [varchar](max) NOT NULL,
		[FilePath] [varchar] (100)  NOT NULL,
		[AuthorId] [bigint]  NOT NULL,
		[SubjectId] [bigint]  NOT NULL,
		[IsDeleted] [bit] NOT NULL DEFAULT 0,
		[DateCreated] [datetime] NOT NULL,
		[DateModified] [datetime] NOT NULL,
		[LastModifiedBy] [bigint] NOT NULL,
		CONSTRAINT [PK_Ispit] PRIMARY KEY CLUSTERED 
		(
			   [Id] ASC
		) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
		CONSTRAINT [FK_Ispit_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[User](Id),
		CONSTRAINT [FK_Ispit_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subject](Id),
	);
END