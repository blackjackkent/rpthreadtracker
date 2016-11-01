using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Infrastructure.Services
{
    public class ExporterService : IExporterService
    {
        public byte[] GetWorkbookBytes(bool includeArchived, int? userId, IEnumerable<BlogDto> blogs, IThreadService threadService, IRepository<Thread> threadRepository, IRepository<Blog> blogRepository)
        {
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
                var threadIds = threadService.GetThreadIdsByBlogId(blog.UserBlogId, threadRepository);
                int i = 2;
                foreach (var threadId in threadIds)
                {
                    var thread = threadService.GetById(threadId.GetValueOrDefault(), blogRepository, threadRepository, null, true);
                    worksheet.Cells["A" + i].Value = blog.BlogShortname;
                    worksheet.Cells["B" + i].Value = Convert.ToInt64(thread.PostId);
                    worksheet.Cells["C" + i].Value = thread.UserTitle;
                    worksheet.Cells["D" + i].Value = thread.WatchedShortname;
                    worksheet.Cells["E" + i].Value = thread.IsArchived;
                    i++;
                }
                if (includeArchived)
                {
                    var archivedThreadIds = threadService.GetThreadIdsByBlogId(blog.UserBlogId, threadRepository, true);
                    foreach (var threadId in archivedThreadIds)
                    {
                        var thread = threadService.GetById(threadId.GetValueOrDefault(), blogRepository,
                            threadRepository, null, true);
                        worksheet.Cells["A" + i].Value = blog.BlogShortname;
                        worksheet.Cells["B" + i].Value = Convert.ToInt64(thread.PostId);
                        worksheet.Cells["C" + i].Value = thread.UserTitle;
                        worksheet.Cells["D" + i].Value = thread.WatchedShortname;
                        worksheet.Cells["E" + i].Value = thread.IsArchived;
                        i++;
                    }
                }
                worksheet.Cells["A1:E1"].Style.Font.Bold = true;
                worksheet.Cells["A1:E1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                worksheet.Cells["A1:E" + i].AutoFitColumns();
            }
            return package.GetAsByteArray();
        }
    }
}