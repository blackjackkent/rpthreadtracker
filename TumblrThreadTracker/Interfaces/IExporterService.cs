using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Interfaces
{
    public interface IExporterService
    {
	    ExcelPackage GetPackage(IEnumerable<BlogDto> blogs, Dictionary<int, IEnumerable<ThreadDto>> threadDistribution, Dictionary<int, IEnumerable<ThreadDto>> archivedThreadDistribution, bool includeArchived);
    }
}