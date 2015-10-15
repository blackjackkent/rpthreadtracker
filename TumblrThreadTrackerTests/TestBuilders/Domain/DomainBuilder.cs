using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels;

namespace TumblrThreadTrackerTests.TestBuilders.Domain
{
    public abstract class DomainBuilder<TModel, TDto>
        where TDto : IDto<TModel>
        where TModel : DomainModel
    {
        protected TDto Dto;

        protected DomainBuilder(TDto dto)
        {
            Dto = dto;
        }

        public TModel Build()
        {
            return Dto.ToModel();
        }

        public TDto BuildDto()
        {
            return Dto;
        }

        public static implicit operator TModel(DomainBuilder<TModel, TDto> builder)
        {
            return builder.Dto.ToModel();
        }

        public static implicit operator TDto(DomainBuilder<TModel, TDto> builder)
        {
            return builder.Dto;
        }
    }
}