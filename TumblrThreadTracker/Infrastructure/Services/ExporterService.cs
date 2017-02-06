namespace TumblrThreadTracker.Infrastructure.Services
{
	using System.Collections.Generic;
	using System.Drawing;
	using Interfaces;
	using Models.DomainModels.Blogs;
	using Models.DomainModels.Threads;
	using OfficeOpenXml;

	/// <inheritdoc cref="IExporterService"/>
	public class ExporterService : IExporterService
	{
		/// <inheritdoc cref="IExporterService"/>
		public ExcelPackage GetPackage(IEnumerable<BlogDto> blogs, Dictionary<int, IEnumerable<ThreadDto>> threadDistribution, Dictionary<int, IEnumerable<ThreadDto>> archivedThreadDistribution, bool includeArchived)
		{
			var package = new ExcelPackage();
			foreach (var blog in blogs)
			{
				var worksheet = package.Workbook.Worksheets.Add(blog.BlogShortname);
				var hasThreads = false;
				var hasArchivedThreads = false;

				// Add the headers
				worksheet.Cells[1, 1].Value = "Blog Shortname";
				worksheet.Cells[1, 2].Value = "Post ID";
				worksheet.Cells[1, 3].Value = "User Title";
				worksheet.Cells[1, 4].Value = "Watched Shortname";
				worksheet.Cells[1, 5].Value = "Is Archived";
				var i = 2;
				if (threadDistribution.ContainsKey(blog.UserBlogId.GetValueOrDefault()))
				{
					hasThreads = true;
				}

				var threads = hasThreads ? threadDistribution[blog.UserBlogId.GetValueOrDefault()] : new List<ThreadDto>();
				foreach (var thread in threads)
				{
					worksheet.Cells["A" + i].Value = blog.BlogShortname;
					worksheet.Cells["B" + i].Value = thread.PostId;
					worksheet.Cells["C" + i].Value = thread.UserTitle;
					worksheet.Cells["D" + i].Value = thread.WatchedShortname;
					worksheet.Cells["E" + i].Value = thread.IsArchived;
					i++;
				}

				if (includeArchived)
				{
					if (archivedThreadDistribution.ContainsKey(blog.UserBlogId.GetValueOrDefault()))
					{
						hasArchivedThreads = true;
					}

					var archivedThreads = hasArchivedThreads
						                      ? archivedThreadDistribution[blog.UserBlogId.GetValueOrDefault()]
						                      : new List<ThreadDto>();
					foreach (var thread in archivedThreads)
					{
						worksheet.Cells["A" + i].Value = blog.BlogShortname;
						worksheet.Cells["B" + i].Value = thread.PostId;
						worksheet.Cells["C" + i].Value = thread.UserTitle;
						worksheet.Cells["D" + i].Value = thread.WatchedShortname;
						worksheet.Cells["E" + i].Value = thread.IsArchived;
						worksheet.Cells["A" + i + ":E" + i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						worksheet.Cells["A" + i + ":E" + i].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
						worksheet.Cells["A" + i + ":E" + i].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#595959"));
						i++;
					}
				}

				if (!(hasArchivedThreads || hasThreads))
				{
					package.Workbook.Worksheets.Delete(worksheet);
					continue;
				}

				worksheet.Cells["A1:E1"].Style.Font.Bold = true;
				worksheet.Cells["A1:E1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
				worksheet.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
				worksheet.Cells["A1:E" + i].AutoFitColumns();
				worksheet.Cells["B2:B" + i].Style.Numberformat.Format = "0";
			}

			return package;
		}
	}
}