namespace Domain.ThreadTag
{
	using NUnit.Framework;
	using Thread;

	class ThreadTagTests
    {
	    private ThreadTag _threadTag;
	    private const int TagId = 12345;
	    private const string TagText = "This is a tag";
	    private Thread _thread;

	    [SetUp]
	    public void Setup()
	    {
			_threadTag = new ThreadTag();
		    _thread = new Thread();
		}

	    [Test]
	    public void CanGetAndSetTagId()
	    {
			// act
		    _threadTag.TagId = TagId;

		    // assert
		    Assert.That(_threadTag.TagId, Is.EqualTo(TagId));
		}

	    [Test]
	    public void CanGetAndSetTagText()
	    {
		    // act
		    _threadTag.TagText = TagText;

		    // assert
		    Assert.That(_threadTag.TagText, Is.EqualTo(TagText));
		}

	    [Test]
	    public void CanGetAndSetThread()
	    {
		    // act
		    _threadTag.Thread = _thread;

		    // assert
		    Assert.That(_threadTag.Thread, Is.EqualTo(_thread));
	    }
	}
}
