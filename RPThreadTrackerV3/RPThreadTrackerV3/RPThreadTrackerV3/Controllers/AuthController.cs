namespace RPThreadTrackerV3.Controllers
{
	using System;
	using System.IdentityModel.Tokens.Jwt;
	using System.Threading.Tasks;
	using Interfaces.Services;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Logging;
	using Models.RequestModels;

	public class AuthController : Controller
	{
		private readonly ILogger<AuthController> _logger;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IPasswordHasher<IdentityUser> _passwordHasher;
		private readonly IConfigurationRoot _config;
		private readonly IAuthService _authService;

		public AuthController(ILogger<AuthController> logger, SignInManager<IdentityUser> signInManager,
			UserManager<IdentityUser> userManager, IPasswordHasher<IdentityUser> passwordHasher, IConfigurationRoot config, IAuthService authService)
		{
			_logger = logger;
			_signInManager = signInManager;
			_userManager = userManager;
			_passwordHasher = passwordHasher;
			_config = config;
			_authService = authService;
		}

		[HttpPost("api/auth/token")]
		public async Task<IActionResult> CreateToken([FromBody] LoginRequest model)
		{
			try
			{
				var user = await _authService.GetUserByUsernameOrEmail(model.Username, _userManager);
				if (user == null)
				{
					_logger.LogWarning($"Login failure for {model.Username}. No user exists with this username or email address.");
					return BadRequest("Invalid username or password.");
				}
				var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
				if (verificationResult == PasswordVerificationResult.Failed)
				{
					_logger.LogWarning($"Login failure for {model.Username}. Invalid non-legacy password.");
					return BadRequest("Invalid username or password.");
				}
				//@TODO add flow for redirecting user to set new password using Core strength requirements
				//if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
				//{
				//	_logger.LogInformation($"Rehashing legacy password for {model.Username}.");
				//	var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
				//	await _userManager.ResetPasswordAsync(user, resetToken, model.Password);
				//}
				var jwt = await _authService.GenerateJwt(user, _config["Tokens:Key"], _config["Tokens:Issuer"], _config["Tokens:Audience"], _userManager);
				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(jwt),
					expiration = jwt.ValidTo
				});
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error while creating JWT: {ex}");
			}
			return BadRequest("Failed to create JWT.");
		}

		[HttpPost("api/auth/register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest model)
		{
			if (ModelState.IsValid)
			{
				var user = new IdentityUser { UserName = model.Email, Email = model.Email };
				try
				{
					var result = await _userManager.CreateAsync(user, model.Password);
					var roleResult = await _userManager.AddToRoleAsync(user, "User");
					if (result.Succeeded && roleResult.Succeeded)
					{
						_logger.LogInformation(3, "User created a new account with password.");
						return Ok();
					}
					return BadRequest(result);
				}
				catch (Exception e)
				{
					return BadRequest(e);
				}
			}
			return BadRequest();
		}
	}
}
