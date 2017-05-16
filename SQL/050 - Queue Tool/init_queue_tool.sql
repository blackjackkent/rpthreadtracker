ALTER TABLE dbo.UserThread ADD MarkedQueued DateTime NULL;  
ALTER TABLE dbo.UserProfile ADD AllowMarkQueued bit NOT NULL DEFAULT 0;