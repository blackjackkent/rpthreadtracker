namespace RPThreadTracker
{
	using System.Configuration;
	using System.Reflection;
	using System.Web.Configuration;
	using Infrastructure.Filters;

	/// <summary>
	/// Class defining setup of WebAPI Filters
	/// </summary>
	[ExcludeFromCoverage]
	public static class KeyConfig
	{
		/// <summary>
		 /// Register a standardized machinekey contained in
		 /// appsettings so it does not have to be stored in source control.
		 /// </summary>
		public static void ConfigureMachineKey()
		{
			var mksType = typeof(MachineKeySection);
			var mksSection = ConfigurationManager.GetSection("system.web/machineKey") as MachineKeySection;
			var resetMethod = mksType.GetMethod("Reset", BindingFlags.NonPublic | BindingFlags.Instance);

			var newConfig = new MachineKeySection();
			newConfig.ApplicationName = mksSection.ApplicationName;
			newConfig.CompatibilityMode = mksSection.CompatibilityMode;
			newConfig.DataProtectorType = mksSection.DataProtectorType;
			newConfig.Validation = mksSection.Validation;

			newConfig.ValidationKey = ConfigurationManager.AppSettings["MK_ValidationKey"];
			newConfig.DecryptionKey = ConfigurationManager.AppSettings["MK_DecryptionKey"];

			resetMethod.Invoke(mksSection, new object[] { newConfig });
		}
	}
}