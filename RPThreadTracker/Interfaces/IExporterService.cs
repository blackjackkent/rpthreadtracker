namespace RPThreadTracker.Interfaces
{
	using System.Collections.Generic;
	using Models.DomainModels.Blogs;
	using Models.DomainModels.Threads;
	using OfficeOpenXml;

	/// <summary>
	/// Class responsible for building Excel file of exported thread data
	/// </summary>
	public interface IExporterService
	{
		/// <summary>
		/// Builds ExcelPackage object which will be transmitted to user as Excel file
		/// </summary>
		/// <param name="blogs">Blogs belonging to user to be exported</param>
		/// <param name="threadDistribution">Distribution of thread objects by blog ID</param>
		/// <param name="archivedThreadDistribution">Distribution of archived thread objects by blog ID</param>
		/// <param name="includeArchived">Whether or not to include archived threads in export</param>
		/// <returns><see cref="ExcelPackage"/> object containing exported thread data to be viewed as Excel file</returns>
		ExcelPackage GetPackage(IEnumerable<BlogDto> blogs, Dictionary<int, IEnumerable<ThreadDto>> threadDistribution, Dictionary<int, IEnumerable<ThreadDto>> archivedThreadDistribution, bool includeArchived);
	}
}