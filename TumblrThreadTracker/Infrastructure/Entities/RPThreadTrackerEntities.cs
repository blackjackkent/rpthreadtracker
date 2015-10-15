using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Interfaces;

namespace TumblrThreadTracker.Infrastructure
{
    public partial class RPThreadTrackerEntities : IThreadTrackerContext
    {
        public void Commit()
        {
            SaveChanges();
        }
    }
}