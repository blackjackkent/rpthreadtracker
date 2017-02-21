namespace RPThreadTracker.Models.RequestModels
{
	using System.Collections.Generic;
	using System.IO;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Formatting;
	using System.Net.Http.Headers;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Web.Http;
	using System.Web.Http.Results;
	using Infrastructure.Filters;
	using OfficeOpenXml;

	/// <inheritdoc cref="OkNegotiatedContentResult{T}"/>
	public class ExportStreamResult : OkNegotiatedContentResult<ExcelPackage>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExportStreamResult"/> class
		/// </summary>
		/// <param name="content">ExcelPackage value to be included in the result</param>
		/// <param name="controller">Calling controller returning the result</param>
		public ExportStreamResult(ExcelPackage content, ApiController controller)
			: base(content, controller)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExportStreamResult"/> class
		/// </summary>
		/// <param name="content">ExcelPackage value to be included in the result</param>
		/// <param name="contentNegotiator">Content negotiator object</param>
		/// <param name="request">Request message which led to this result</param>
		/// <param name="formatters">Formatters used to format the content</param>
		[ExcludeFromCoverage]
		public ExportStreamResult(ExcelPackage content, IContentNegotiator contentNegotiator, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
			: base(content, contentNegotiator, request, formatters)
		{
		}

		/// <summary>
		/// Gets or sets user ID for filename
		/// </summary>
		/// <value>
		/// Integer value of user ID for user who owns threads to be exported
		/// </value>
		public int UserId { get; set; }

		/// <inheritdoc cref="OkNegotiatedContentResult{T}" />
		public override async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			var result = new HttpResponseMessage(HttpStatusCode.OK);
			using (var ms = new MemoryStream())
			{
				var bytes = Content.GetAsByteArray();
				ms.Write(bytes, 0, bytes.Length);
				var copy = new MemoryStream(ms.ToArray());
				copy.Seek(0, SeekOrigin.Begin);
				result.Content = new StreamContent(copy);
				result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
				result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
				result.Content.Headers.Add("x-filename", UserId + "_Export.xlsx");
				return result;
			}
		}
	}
}