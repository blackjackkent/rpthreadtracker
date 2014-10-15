USE TumblrThreadTracker;
GO

ALTER TABLE dbo.UserThread
	ADD WatchedShortname varchar(50) NULL;
GO