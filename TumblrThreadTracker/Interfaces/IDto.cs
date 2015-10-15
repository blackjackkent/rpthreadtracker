using TumblrThreadTracker.Models.DomainModels;

namespace TumblrThreadTracker.Interfaces
{
    public interface IDto<out TModel> where TModel : DomainModel
    {
        TModel ToModel();
    }
}