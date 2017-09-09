SET IDENTITY_INSERT dbo.Characters ON;

INSERT INTO dbo.characters (
CharacterId
, UserId
, BlogShortname
, IsOnHiatus)
SELECT
	b.UserBlogId as CharacterId,
	b.UserId,
	b.BlogShortname,
	b.OnHiatus as IsOnHiatus
    FROM dbo.userblog b
	WHERE EXISTS (SELECT
        1
    FROM dbo.aspnetusers
    WHERE id = b.UserId)

SET IDENTITY_INSERT dbo.Characters OFF;