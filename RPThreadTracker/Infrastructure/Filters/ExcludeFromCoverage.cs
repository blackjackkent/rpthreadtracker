namespace RPThreadTracker.Infrastructure.Filters
{
	using System;

	/// <summary>
	/// Attribute applied to classes/methods/properties which should
	/// be excluded from code coverage metrics.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor)]
	public class ExcludeFromCoverage : Attribute
	{
	}
}