USE RPThreadTracker;
GO

IF OBJECT_ID('dbo.UserSettings', 'U') IS NOT NULL 
  DROP TABLE dbo.UserSettings; 
IF OBJECT_ID('dbo.AspNetUsers', 'U') IS NOT NULL 
  DROP TABLE dbo.AspNetUsers; 
IF OBJECT_ID('dbo.Log', 'U') IS NOT NULL 
  DROP TABLE dbo.Log; 
GO