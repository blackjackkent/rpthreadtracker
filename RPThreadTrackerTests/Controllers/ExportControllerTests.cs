namespace RPThreadTrackerTests.Controllers
{
	using System.Collections.Generic;
	using System.Security.Claims;
	using Moq;
	using NUnit.Framework;
	using OfficeOpenXml;
	using RPThreadTracker.Controllers;
	using RPThreadTracker.Interfaces;
	using RPThreadTracker.Models.DomainModels.Blogs;
	using RPThreadTracker.Models.DomainModels.Threads;
	using RPThreadTracker.Models.RequestModels;

	internal class ExportControllerTests
	{
		private ExportController _exportController;
		private Mock<IRepository<Blog>> _userBlogRepository;
		private Mock<IRepository<Thread>> _userThreadRepository;
		private Mock<IWebSecurityService> _webSecurityService;
		private Mock<IBlogService> _blogService;
		private Mock<IThreadService> _threadService;
		private Mock<IExporterService> _exporterService;

		[SetUp]
		public void Setup()
		{
			_userBlogRepository = new Mock<IRepository<Blog>>();
			_userThreadRepository = new Mock<IRepository<Thread>>();
			_webSecurityService = new Mock<IWebSecurityService>();
			_blogService = new Mock<IBlogService>();
			_threadService = new Mock<IThreadService>();
			_exporterService = new Mock<IExporterService>();
			_exportController = new ExportController(_userBlogRepository.Object, _userThreadRepository.Object, _webSecurityService.Object, _blogService.Object, _threadService.Object, _exporterService.Object);
		}

		[Test]
		public void Get_RequestValid_ExportsPackage()
		{
			// Arrange
			const int userId = 5;
			const bool includeHiatusedBlogs = true;
			const bool includeArchived = true;
			var blogs = new List<BlogDto>();
			var threadDistribution = new Dictionary<int, IEnumerable<ThreadDto>>();
			var archivedThreadDistribution = new Dictionary<int, IEnumerable<ThreadDto>>();
			var package = new ExcelPackage();
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(userId);
			_blogService.Setup(s => s.GetBlogsByUserId(userId, _userBlogRepository.Object, includeHiatusedBlogs)).Returns(blogs);
			_threadService.Setup(s => s.GetThreadDistribution(blogs, _userThreadRepository.Object, false)).Returns(threadDistribution);
			_threadService.Setup(s => s.GetThreadDistribution(blogs, _userThreadRepository.Object, true)).Returns(archivedThreadDistribution);
			_exporterService.Setup(s => s.GetPackage(blogs, threadDistribution, archivedThreadDistribution, includeArchived)).Returns(package);

			// Act
			var result = _exportController.Get(includeArchived, includeHiatusedBlogs);

			// Assert
			_threadService.Verify(s => s.GetThreadDistribution(blogs, _userThreadRepository.Object, false), Times.Once);
			_threadService.Verify(s => s.GetThreadDistribution(blogs, _userThreadRepository.Object, true), Times.Once);
			_exporterService.Verify(s => s.GetPackage(blogs, threadDistribution, archivedThreadDistribution, includeArchived), Times.Once);
			Assert.That(result, Is.TypeOf<ExportStreamResult>());
			var response = result as ExportStreamResult;
			Assert.That(response.UserId, Is.EqualTo(userId));
		}
	}
}
