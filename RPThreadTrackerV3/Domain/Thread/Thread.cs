using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Thread
{
	using Character;

	class Thread
    {
	    public int ThreadId { get; set; }
	    public Character Character { get; set; }
	    public string PostId { get; set; }
	    public string UserTitle { get; set; }
	    public string WatchedShortname { get; set; }
	    public bool IsArchived { get; set; }
	    public DateTime? DateMarkedQueued { get; set; }
    }
}
