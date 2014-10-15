USE TumblrThreadTracker;
GO

ALTER TABLE dbo.UserThread DROP COLUMN WatchedShortname
GO

DELETE FROM dbo.UserThread WHERE userthreadid >= 35