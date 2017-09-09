namespace Domain.ProfileSettings
{
	using UserProfile;

	class ProfileSettings
    {
	    public int SettingsId { get; set; }
	    public UserProfile User { get; set; }
	    public bool ShowDashboardThreadDistribution { get; set; }
	    public bool AllowMarkQueued { get; set; }
	    public bool UseInvertedTheme { get; set; }
    }
}
