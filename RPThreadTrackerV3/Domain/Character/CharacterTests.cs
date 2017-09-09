namespace Domain.Character
{
	using NUnit.Framework;
	using UserProfile;

	class CharacterTests
    {
	    private Character _character;
	    private const int CharacterId = 1234;
	    private UserProfile _user;
	    private const string BlogShortname = "testshortname";
	    private const bool IsOnHiatus = true;

	    [SetUp]
	    public void SetUp()
	    {
		    _character = new Character();
			_user = new UserProfile();
		}

	    [Test]
	    public void CanGetAndSetSettingsId()
	    {
			// act
		    _character.CharacterId = CharacterId;

		    // assert
		    Assert.That(_character.CharacterId, Is.EqualTo(CharacterId));
		}

	    [Test]
	    public void CanGetAndSetUserId()
	    {
		    // act
		    _character.User = _user;

			// assert
			Assert.That(_character.User, Is.EqualTo(_user));
		}

	    [Test]
	    public void CanGetAndSetBlogShortname()
	    {
		    // act
		    _character.BlogShortname = BlogShortname;

		    // assert
		    Assert.That(_character.BlogShortname, Is.EqualTo(BlogShortname));
		}

	    [Test]
	    public void CanGetAndSetIsOnHiatus()
	    {
		    // act
		    _character.IsOnHiatus = IsOnHiatus;

		    // assert
		    Assert.That(_character.IsOnHiatus, Is.EqualTo(IsOnHiatus));
	    }
	}
}
