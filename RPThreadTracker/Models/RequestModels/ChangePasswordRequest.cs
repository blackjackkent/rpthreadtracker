namespace RPThreadTracker.Models.RequestModels
{
	/// <summary>
	/// Request object for changing the password of a user account
	/// </summary>
	public class ChangePasswordRequest
	{
		/// <summary>
		/// Gets or sets the user's confirmation of their requested new password
		/// </summary>
		/// <value>
		/// String confirming the user's requested new password
		/// </value>
		public string ConfirmNewPassword { get; set; }

		/// <summary>
		/// Gets or sets the user's requested new password
		/// </summary>
		/// <value>
		/// String representing the user's requested new password
		/// </value>
		public string NewPassword { get; set; }

		/// <summary>
		/// Gets or sets the user's acknowledgment of their old password
		/// </summary>
		/// <value>
		/// String representing the user's old password authorizing the request
		/// </value>
		public string OldPassword { get; set; }
	}
}