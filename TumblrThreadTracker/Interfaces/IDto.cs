using TumblrThreadTracker.Models;

namespace TumblrThreadTracker.Interfaces
{
    public interface IDto<out TModel> where TModel : Model
    {
        TModel ToModel();
    }
}