namespace Domain.ProfileSettings
{
	using NUnit.Framework;
	using UserProfile;

	class ProfileSettingsTests
    {
	    private ProfileSettings _profileSettings;
	    private const int SettingsId = 12345;
	    private UserProfile _user;
	    private const bool ShowDashboardThreadDistribution = true;
	    private const bool UseInvertedTheme = true;
	    private const bool AllowMarkQueued = true;

	    [SetUp]
	    public void Setup()
	    {
		    _profileSettings = new ProfileSettings();
			_user = new UserProfile();
		}

	    [Test]
	    public void CanGetAndSetSettingsId()
	    {
		    // act
		    _profileSettings.SettingsId = SettingsId;

		    // assert
		    Assert.That(_profileSettings.SettingsId, Is.EqualTo(SettingsId));
	    }

		[Test]
	    public void CanGetAndSetUserId()
	    {
			// act
		    _profileSettings.User = _user;

		    // assert
		    Assert.That(_profileSettings.User, Is.EqualTo(_user));
		}

	    [Test]
	    public void CanGetAndSetShowDashboardThreadDistribution()
	    {
		    // act
		    _profileSettings.ShowDashboardThreadDistribution = ShowDashboardThreadDistribution;

		    // assert
		    Assert.That(_profileSettings.ShowDashboardThreadDistribution, Is.EqualTo(ShowDashboardThreadDistribution));
		}

	    [Test]
	    public void CanGetAndSetAllowMarkQueued()
	    {
		    // act
		    _profileSettings.AllowMarkQueued = AllowMarkQueued;

		    // assert
		    Assert.That(_profileSettings.AllowMarkQueued, Is.EqualTo(AllowMarkQueued));
		}

	    [Test]
	    public void CanGetAndSetUseInvertedTheme()
	    {
		    // act
		    _profileSettings.UseInvertedTheme = UseInvertedTheme;

		    // assert
		    Assert.That(_profileSettings.UseInvertedTheme, Is.EqualTo(UseInvertedTheme));
	    }
	}
}
