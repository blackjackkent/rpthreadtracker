namespace Domain.Character
{
	using UserProfile;

	class Character
    {
	    public int CharacterId { get; set; }
	    public UserProfile User { get; set; }
	    public string BlogShortname { get; set; }
	    public bool IsOnHiatus { get; set; }
    }
}
