using System.Linq;
using AutoMapper;
using TumblrThreadTracker.Infrastructure;
using TumblrThreadTracker.Models.DomainModels.Account;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker
{
    public static class AutoMapperConfiguration
    {
        static AutoMapperConfiguration()
        {
            ConfigureModelToDto();
            ConfigureDtoToModel();
            ConfigureEntityToModel();
            ConfigureModelToEntity();
            ConfigureEntityToDto();
            ConfigureDtoToEntity();
        }

        private static void ConfigureModelToEntity()
        {
            Mapper.CreateMap<Blog, UserBlog>()
                .ForMember(dest => dest.UserProfile, m => m.Ignore())
                .ForMember(dest => dest.UserThreads, m => m.Ignore());
            Mapper.CreateMap<User, UserProfile>()
                .ForMember(dest => dest.UserBlogs, m => m.Ignore());
            Mapper.CreateMap<Thread, UserThread>()
                .ForMember(dest => dest.UserBlog, m => m.Ignore());
            Mapper.CreateMap<WebpagesMembership, webpages_Membership>();
        }

        private static void ConfigureEntityToModel()
        {
            Mapper.CreateMap<UserBlog, Blog>();
            Mapper.CreateMap<UserProfile, User>();
            Mapper.CreateMap<UserThread, Thread>()
                .ForMember(dest => dest.ThreadTags, m => m.MapFrom(src => src.UserThreadTags.Select(t => t.TagText)));
            Mapper.CreateMap<webpages_Membership, WebpagesMembership>();
        }

        private static void ConfigureEntityToDto()
        {
            Mapper.CreateMap<UserBlog, BlogDto>();
            Mapper.CreateMap<UserProfile, UserDto>();
            Mapper.CreateMap<UserThread, ThreadDto>()
                .ForMember(dest => dest.ThreadTags, m => m.MapFrom(src => src.UserThreadTags.Select(t => t.TagText)));
            Mapper.CreateMap<webpages_Membership, WebpagesMembershipDto>();
        }

        private static void ConfigureDtoToEntity()
        {
            Mapper.CreateMap<BlogDto, UserBlog>()
                .ForMember(dest => dest.UserProfile, m => m.Ignore())
                .ForMember(dest => dest.UserThreads, m => m.Ignore());
            Mapper.CreateMap<UserDto, UserProfile>()
                .ForMember(dest => dest.UserBlogs, m => m.Ignore());
            Mapper.CreateMap<ThreadDto, UserThread>()
                .ForMember(dest => dest.UserBlog, m => m.Ignore());
            Mapper.CreateMap<WebpagesMembershipDto, webpages_Membership>();
        }

        private static void ConfigureDtoToModel()
        {
            Mapper.CreateMap<BlogDto, Blog>();
            Mapper.CreateMap<UserDto, User>();
            Mapper.CreateMap<ThreadDto, Thread>();
            Mapper.CreateMap<WebpagesMembershipDto, WebpagesMembership>();
        }

        private static void ConfigureModelToDto()
        {
            Mapper.CreateMap<Blog, BlogDto>();
            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<Thread, ThreadDto>();
            Mapper.CreateMap<WebpagesMembership, WebpagesMembershipDto>();
        }

        /// <summary>
        ///     Configures Auto Mapper.  Call this method as often as needed.  All configuration occurs in this class's
        ///     static constructor so not additional performance penalty is incurred with subsequent calls.
        /// </summary>
        public static void Configure()
        {
            // Intentionally left blank. Calling this (or any member of this class) insures the static constructor is hit once per AppDomain.
        }
    }
}