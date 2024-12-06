using System.Linq.Expressions;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();

        Task<T> GetById(int id);

        Task<bool> Add(T entity);

        Task<bool> Delete(int id);

        Task<bool> Update(T entity);
        Task<bool> BulkInsert(List<T> entity);
        Task<bool> BulkUpdate(List<T> entity);

        Task<IEnumerable<T>> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        void DeleteMulti(Expression<Func<T, bool>> where);

        Task<int> Count(Expression<Func<T, bool>> where);

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        Task<bool> RemoveRange(IEnumerable<T> entities);
    }
}