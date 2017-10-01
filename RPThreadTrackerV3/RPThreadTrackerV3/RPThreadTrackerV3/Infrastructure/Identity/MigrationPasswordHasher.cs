namespace RPThreadTrackerV3.Infrastructure.Identity
{
	using System;
	using System.Security.Cryptography;
	using System.Text;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

	public class MigrationPasswordHasher : PasswordHasher<IdentityUser>
	{
		public override PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword,
			string providedPassword)
		{
			throw new NotImplementedException();
		}
	}
}
