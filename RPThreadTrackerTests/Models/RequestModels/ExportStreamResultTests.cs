namespace RPThreadTrackerTests.Models.RequestModels
{
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Web.Http;
	using Moq;
	using NUnit.Framework;
	using OfficeOpenXml;
	using RPThreadTracker.Models.RequestModels;

	[TestFixture]
    internal class ExportStreamResultTests
    {
		[Test]
	    public async Task ExecuteAsync_BuildsValidResponse()
	    {
		    // Arrange
		    var userId = 5;
		    var package = new ExcelPackage();
		    package.Workbook.Worksheets.Add("test");
			var expectedContent = package.GetAsByteArray();
		    var controller = new Mock<ApiController>();
		    var expectedFileName = userId + "_Export.xlsx";
			var exportStreamResult = new ExportStreamResult(expectedContent, controller.Object)
			{
				UserId = userId
			};

			// Act
			var response = await exportStreamResult.ExecuteAsync(CancellationToken.None);
			var responseBytes = await response.Content.ReadAsByteArrayAsync();

			// Assert
			Assert.That(responseBytes, Is.EqualTo(expectedContent));
			Assert.That(response.Content.Headers.ContentType.MediaType, Is.EqualTo("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
			Assert.That(response.Content.Headers.ContentDisposition.DispositionType, Is.EqualTo("attachment"));
			Assert.That(response.Content.Headers.FirstOrDefault(h => h.Key == "x-filename").Value.FirstOrDefault(), Is.EqualTo(expectedFileName));
		}
	}
}
