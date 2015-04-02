using System.Collections.Generic;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.ServiceModels;
using TumblrThreadTrackerTests.TestBuilders.Service;
using Blog = TumblrThreadTracker.Models.ServiceModels.Blog;

namespace TumblrThreadTrackerTests.TestBuilders.Domain
{
    public class ServiceObjectBuilder : ServiceBuilder<ServiceObject>
    {
        public ServiceObjectBuilder()
        {
        }

        private static ServiceObject GetDefaultValues()
        {
            return new ServiceObject
            {
                meta = new ServiceMeta
                {
                    msg = "testmsg",
                    status = 3
                },
                response = new ServiceResponse
                {
                    blog = new Blog
                    {
                        ask = false,
                        ask_anon = false,
                        description = "this is a test blog",
                        likes = 12345,
                        name = "blogname",
                        posts = 23456,
                        title = "test title",
                        updated = 234567
                    },
                    posts = new List<Post>
                    {
                        new Post
                        {
                            blog_name = "test blogname",
                            bookmarklet = false,
                            date = "date",
                            format = "formatstring",
                            id = 12345676,
                            liked = true,
                            mobile = false,
                            notes = new List<Note>
                            {
                                new Note
                                {
                                    added_text = "test added text",
                                    blog_name = "test blog name",
                                    blog_url = "test blog url",
                                    post_id = "test post id",
                                    timestamp = 12345678,
                                    type = "test type"
                                }
                            },
                            post_url = "test url",
                            reblog_key = "test key",
                            source_title = "test source title",
                            source_url = "test source url",
                            state = "test state",
                            tags = new List<string> { "tag1", "tag2"},
                            timestamp = 123456,
                            title = "test title",
                            total_posts = 1234,
                            type = "test type"
                        }
                    }
                }
            };
        }

        public override ServiceObject Build()
        {
            return GetDefaultValues();
        }
    }
}