namespace RPThreadTracker.Interfaces
{
	using Models.DomainModels;

	/// <summary>
	/// Interface for Data Transfer representations of <see cref="DomainModel"/> objects
	/// </summary>
	/// <typeparam name="TModel">Model to be represented for transfer</typeparam>
	public interface IDto<out TModel>
		where TModel : DomainModel
	{
		/// <summary>
		/// Converts object to <see cref="TModel"/>
		/// </summary>
		/// <returns><see cref="TModel"/> object corresponding to this DTO</returns>
		TModel ToModel();
	}
}