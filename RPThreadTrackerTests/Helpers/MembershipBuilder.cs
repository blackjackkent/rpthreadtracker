namespace RPThreadTrackerTests.Helpers
{
	using RPThreadTracker.Models.DomainModels.Account;

	public class MembershipBuilder
	{
		private string _passwordVerificationToken = "123456";
		private int _userId = 4;

		public Membership Build()
		{
			return new Membership
			{
				UserId = _userId,
				PasswordVerificationToken = _passwordVerificationToken
			};
		}

		public MembershipDto BuildDto()
		{
			return new MembershipDto
			{
				PasswordVerificationToken = _passwordVerificationToken,
				UserId = _userId
			};
		}

		public MembershipBuilder WithUserId(int userId)
		{
			_userId = userId;
			return this;
		}

		public MembershipBuilder WithPasswordVerificationToken(string passwordVerificationToken)
		{
			_passwordVerificationToken = passwordVerificationToken;
			return this;
		}
	}
}
