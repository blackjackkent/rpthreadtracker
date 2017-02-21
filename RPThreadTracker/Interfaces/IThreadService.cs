﻿namespace RPThreadTracker.Interfaces
{
	using System.Collections.Generic;
	using Models.DomainModels.Blogs;
	using Models.DomainModels.Threads;

	/// <summary>
	/// Class which facilitates interaction with repository layer
	/// for retrieving <see cref="Thread"/> data
	/// </summary>
	public interface IThreadService
	{
		/// <summary>
		/// Writes new <see cref="Thread"/> object to database
		/// </summary>
		/// <param name="dto"><see cref="ThreadDto"/> object containing information about thread to be created.</param>
		/// <param name="threadRepository">Repository object containing database connection</param>
		/// <returns><see cref="ThreadDto" /> object returned by insert</returns>
		ThreadDto AddNewThread(ThreadDto dto, IRepository<Thread> threadRepository);

		/// <summary>
		/// Removes thread with passed identifier from database
		/// </summary>
		/// <param name="userThreadId">Unique identifier of thread to be deleted</param>
		/// <param name="threadRepository">Repository object containing database connection</param>
		void DeleteThread(int userThreadId, IRepository<Thread> threadRepository);

		/// <summary>
		/// Gets a single thread object by its unique identifier
		/// </summary>
		/// <param name="id">Unique identifier for the thread to be retrieved</param>
		/// <param name="blogRepository">Repository object containing database connection for blog info</param>
		/// <param name="threadRepository">Repository object containing database connection for thread info</param>
		/// <param name="tumblrClient">Wrapper class for HTTP client connection to Tumblr API</param>
		/// <returns><see cref="ThreadDto"/> object containing information for requested ID</returns>
		ThreadDto GetById(int id, IRepository<Blog> blogRepository, IRepository<Thread> threadRepository, ITumblrClient tumblrClient);

		/// <summary>
		/// Gets five most recent #news posts from the Tracker blog on Tumblr
		/// </summary>
		/// <param name="tumblrClient">Wrapper class for HTTP client connection to Tumblr API</param>
		/// <param name="configurationService">Wrapper class for app config information</param>
		/// <returns>List of five <see cref="ThreadDto"/> objects representing five posts</returns>
		IEnumerable<ThreadDto> GetNewsThreads(ITumblrClient tumblrClient, IConfigurationService configurationService);

		/// <summary>
		/// Gets all IDs for tracked threads belonging to a particular user account
		/// </summary>
		/// <param name="userId">Unique identifier of user whose info should be retrieved</param>
		/// <param name="threadRepository">Repository object containing database connection</param>
		/// <param name="isArchived">Whether or not to retrieve archived threads</param>
		/// <returns>List of integer identifiers for tracked threads</returns>
		IEnumerable<int?> GetThreadIdsByUserId(int? userId, IRepository<Thread> threadRepository, bool isArchived = false);

		/// <summary>
		/// Gets <see cref="ThreadDto"/> representations of all threads tracked on a particular blog
		/// </summary>
		/// <param name="blog"><see cref="BlogDto"/> object for which to retrieve thread information</param>
		/// <param name="threadRepository">Repository object containing database connection</param>
		/// <param name="isArchived">Whether or not to retrieve archived threads</param>
		/// <returns>List of <see cref="ThreadDto"/> objects</returns>
		IEnumerable<ThreadDto> GetThreadsByBlog(BlogDto blog, IRepository<Thread> threadRepository, bool isArchived = false);

		/// <summary>
		/// Gets integer IDs of all threads tracked on a particular blog
		/// </summary>
		/// <param name="blog"><see cref="BlogDto"/> object for which to retrieve thread information</param>
		/// <param name="threadRepository">Repository object containing database connection</param>
		/// <param name="isArchived">Whether or not to retrieve archived threads</param>
		/// <returns>List of <see cref="ThreadDto"/> objects</returns>
		IEnumerable<int?> GetThreadIdsByBlog(BlogDto blog, IRepository<Thread> threadRepository, bool isArchived = false);

		/// <summary>
		/// Updates existing thread with passed property information
		/// </summary>
		/// <param name="dto"><see cref="ThreadDto"/> object containing data to be updated on database object</param>
		/// <param name="threadRepository">Repository object containing database connection</param>
		void UpdateThread(ThreadDto dto, IRepository<Thread> threadRepository);

		/// <summary>
		/// Determines whether or not a particular user is the owner of a particular thread
		/// </summary>
		/// <param name="userId">Unique identifier of user account to check</param>
		/// <param name="threadId">Unique identifier of thread to check</param>
		/// <param name="threadRepository">Repository object containing database connection</param>
		/// <returns>True if user is associated with thread, false if not</returns>
		bool UserOwnsThread(int userId, int threadId, IRepository<Thread> threadRepository);

		/// <summary>
		/// Retrieves thread information organized by userblogid
		/// </summary>
		/// <param name="blogs">Blogs to retrieve thread information for</param>
		/// <param name="threadRepository">Repository object containing database connection</param>
		/// <param name="isArchived">Whether the threads retrieved should be IsArchived = true or not</param>
		/// <returns>Dictionary of thread information organized by userblogid</returns>
		Dictionary<int, IEnumerable<ThreadDto>> GetThreadDistribution(IEnumerable<BlogDto> blogs, IRepository<Thread> threadRepository, bool isArchived);

		/// <summary>
		/// Populates information about the last post in the thread
		/// based on Tumblr note information
		/// </summary>
		/// <param name="thread"><see cref="ThreadDto"/> to be hydrated</param>
		/// <param name="post"><see cref="IPost"/> to use for last post information</param>
		/// <returns><see cref="ThreadDto" /> object hydrated with latest post info</returns>
		ThreadDto HydrateThread(ThreadDto thread, IPost post);
	}
}