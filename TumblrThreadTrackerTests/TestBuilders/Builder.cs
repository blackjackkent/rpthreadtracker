using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.DomainModels;

namespace TumblrThreadTrackerTests.TestBuilders
{
    public abstract class Builder<TModel, TDto>
        where TDto : IDto<TModel>
        where TModel : Model
    {
        protected TDto Dto;

        protected Builder(TDto dto)
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

        public static implicit operator TModel(Builder<TModel, TDto> builder)
        {
            return builder.Dto.ToModel();
        }

        public static implicit operator TDto(Builder<TModel, TDto> builder)
        {
            return builder.Dto;
        }
    }
}