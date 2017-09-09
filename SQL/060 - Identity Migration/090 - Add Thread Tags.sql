USE [RPThreadTracker]
GO

/****** Object:  Table [dbo].[UserThreadTag]    Script Date: 9/9/2017 4:39:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ThreadTags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[TagText] [varchar](140) NOT NULL,
	[ThreadID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ThreadTags]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ThreadTags_dbo.Threads_ThreadId] FOREIGN KEY([ThreadID])
REFERENCES [dbo].[Threads] ([ThreadId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ThreadTags] CHECK CONSTRAINT [FK_dbo.ThreadTags_dbo.Threads_ThreadId]
GO


