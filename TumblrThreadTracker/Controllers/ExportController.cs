using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using OfficeOpenXml;
using TumblrThreadTracker.Infrastructure.Filters;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Controllers
{
    [RedirectOnMaintenance]
    [Authorize]
    public class ExportController : ApiController
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IRepository<Thread> _threadRepository;
        private readonly IWebSecurityService _webSecurityService;
        private readonly IBlogService _blogService;
        private readonly IThreadService _threadService;
        private readonly ITumblrClient _tumblrClient;

        public ExportController(IRepository<Blog> userBlogRepository, IRepository<Thread> userThreadRepository, IWebSecurityService webSecurityService, IBlogService blogService, IThreadService threadService, ITumblrClient tumblrClient)
        {
            _blogRepository = userBlogRepository;
            _threadRepository = userThreadRepository;
            _webSecurityService = webSecurityService;
            _blogService = blogService;
            _threadService = threadService;
            _tumblrClient = tumblrClient;
        }

        public async Task<HttpResponseMessage> Get()
        {
            var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
            var blogs = _blogService.GetBlogsByUserId(userId, _blogRepository, true);
            var package = new ExcelPackage();
            foreach (var blog in blogs)
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(blog.BlogShortname);
                //Add the headers
                worksheet.Cells[1, 1].Value = "Blog Shortname";
                worksheet.Cells[1, 2].Value = "Post ID";
                worksheet.Cells[1, 3].Value = "User Title";
                worksheet.Cells[1, 4].Value = "Watched Shortname";
                worksheet.Cells[1, 5].Value = "Is Archived";
                var threadIds = _threadService.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository);
                var archivedThreadIds = _threadService.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository, true);
                int i = 2;
                foreach (var threadId in threadIds)
                {
                    var thread = _threadService.GetById(threadId.GetValueOrDefault(), _blogRepository,
                        _threadRepository, _tumblrClient, true);
                    worksheet.Cells["A" + i].Value = blog.BlogShortname;
                    worksheet.Cells["B" + i].Value = Convert.ToInt64(thread.PostId);
                    worksheet.Cells["C" + i].Value = thread.UserTitle;
                    worksheet.Cells["D" + i].Value = thread.WatchedShortname;
                    worksheet.Cells["E" + i].Value = thread.IsArchived;
                    i++;
                }
                foreach (var threadId in archivedThreadIds)
                {
                    var thread = _threadService.GetById(threadId.GetValueOrDefault(), _blogRepository,
                        _threadRepository, _tumblrClient, true);
                    worksheet.Cells["A" + i].Value = blog.BlogShortname;
                    worksheet.Cells["B" + i].Value = Convert.ToInt64(thread.PostId);
                    worksheet.Cells["C" + i].Value = thread.UserTitle;
                    worksheet.Cells["D" + i].Value = thread.WatchedShortname;
                    worksheet.Cells["E" + i].Value = thread.IsArchived;
                    i++;
                }
                worksheet.Cells["A1:E1"].Style.Font.Bold = true;
                worksheet.Cells["A1:E1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                worksheet.Cells["A1:E" + i].AutoFitColumns();
            }
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] bytes = package.GetAsByteArray();
                ms.Write(bytes, 0, (int) bytes.Length);
                MemoryStream copy = new MemoryStream(ms.ToArray());
                copy.Seek(0, SeekOrigin.Begin);
                result.Content = new StreamContent(copy);
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.Add("x-filename", userId + "Export.xlsx");
                return result;
            }
        }
    }
}