IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_Name = 'FavoriteSubject'))
BEGIN
	CREATE TABLE [dbo].[FavoriteSubject](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[UserId] [bigint] NOT NULL,
		[SubjectId] [bigint] NOT NULL
		CONSTRAINT [PK_FavoriteSubject] PRIMARY KEY CLUSTERED 
		(
			   [Id] ASC
		) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
		CONSTRAINT [FK_FavoriteSubject_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User](Id),
		CONSTRAINT [FK_FavoriteSubject_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subject](Id),
	);
END