using System;

namespace Domain.Thread
{
	using Character;
	using NUnit.Framework;

	class ThreadTests
    {
	    private Thread _thread;
	    private const int ThreadId = 1234;
	    private Character _character;
	    private const string PostId = "1234567";
	    private const string UserTitle = "This is a test thread";
	    private const string WatchedShortname = "TestPartner";
	    private const bool IsArchived = true;
	    private DateTime? _dateMarkedQueued;

	    [SetUp]
	    public void Setup()
	    {
		    _thread = new Thread();
			_character = new Character();
		    _dateMarkedQueued = DateTime.Now;
		}

	    [Test]
	    public void CanGetAndSetThreadId()
	    {
		    // act
		    _thread.ThreadId = ThreadId;

		    // assert
		    Assert.That(_thread.ThreadId, Is.EqualTo(ThreadId));
	    }

		[Test]
	    public void CanGetAndSetCharacterId()
	    {
			// act
		    _thread.Character = _character;

		    // assert
		    Assert.That(_thread.Character, Is.EqualTo(_character));
		}

	    [Test]
	    public void CanGetAndSetUserTitle()
	    {
		    // act
		    _thread.UserTitle = UserTitle;

		    // assert
		    Assert.That(_thread.UserTitle, Is.EqualTo(UserTitle));
		}

	    [Test]
	    public void CanGetAndSetWatchedShortname()
	    {
		    // act
		    _thread.WatchedShortname = WatchedShortname;

		    // assert
		    Assert.That(_thread.WatchedShortname, Is.EqualTo(WatchedShortname));
	    }

		[Test]
	    public void CanGetAndSetPostId()
	    {
		    // act
		    _thread.PostId = PostId;

		    // assert
		    Assert.That(_thread.PostId, Is.EqualTo(PostId));
		}

	    [Test]
	    public void CanGetAndSetIsArchived()
	    {
		    // act
		    _thread.IsArchived = IsArchived;

		    // assert
		    Assert.That(_thread.IsArchived, Is.EqualTo(IsArchived));
		}

	    [Test]
	    public void CanGetAndSetDateMarkedQueued()
	    {
		    // act
		    _thread.DateMarkedQueued = _dateMarkedQueued;

		    // assert
		    Assert.That(_thread.DateMarkedQueued, Is.EqualTo(_dateMarkedQueued));
	    }

	}
}
