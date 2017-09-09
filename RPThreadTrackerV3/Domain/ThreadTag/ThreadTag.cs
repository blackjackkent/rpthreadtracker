using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ThreadTag
{
	using Thread;

	class ThreadTag
    {
	    public int TagId { get; set; }
	    public string TagText { get; set; }
	    public Thread Thread { get; set; }
    }
}
