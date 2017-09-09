SET IDENTITY_INSERT dbo.ThreadTags ON;

INSERT INTO dbo.ThreadTags (
TagId
, TagText
, ThreadId)
SELECT
	t.TagID,
	t.TagText,
	t.UserThreadID as ThreadId
    FROM dbo.userthreadTag t

SET IDENTITY_INSERT dbo.ThreadTags OFF;