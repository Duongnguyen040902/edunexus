namespace sep.backend.v1.Services.IServices
{
    public interface IBaseService<TSource, TDestination>
    {
        Task<IEnumerable<TDestination>> GetAllAsync();

        Task<TDestination> GetByIdAsync(int id);

        Task<TDestination> CreateAsync(TSource source);

        Task<TDestination> UpdateAsync(int id, TSource source);

        Task<TDestination> DeleteAsync(int id);
    }
}