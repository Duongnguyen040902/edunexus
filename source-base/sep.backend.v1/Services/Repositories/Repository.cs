using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;
using System.Linq.Expressions;

namespace sep.backend.v1.Services.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<T> dbSet;
        protected readonly ILogger<Repository<T>> _logger;

        public Repository(ApplicationContext context, ILogger<Repository<T>> logger)
        {
            _context = context;
            _logger = logger;
            this.dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            try
            {
                return await dbSet.FindAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting entity with id {Id}", id);
                return null;
            }
        }

        public virtual async Task<bool> Add(T entity)
        {
            try
            {
                await dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding entity");
                return false;
            }
        }

        public virtual async Task<bool> Delete(int id)
        {
            try
            {
                var entity = await dbSet.FindAsync(id);
                if (entity != null)
                {
                    dbSet.Remove(entity);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Entity with id {Id} not found for deletion", id);
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting entity with id {Id}", id);
                return false;
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating entity");
                return false;
            }
        }

        // Get multiple entities that satisfy a given condition, with optional related data
        public async Task<IEnumerable<T>> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            try
            {
                // Start building the query with the base DbSet<T>
                IQueryable<T> query = dbSet;
                // Include related entities if provided (for example, loading related tables)
                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                // Apply the filtering condition (predicate) and return the result as a list
                return await query.Where(predicate).ToListAsync();
            }
            catch (Exception e)
            {
                // Log the error and return an empty collection if something goes wrong
                _logger.LogError(e, "Error retrieving multiple entities");
                return Enumerable.Empty<T>();
            }
        }

        // Get a single entity that matches a given condition, with optional related data
        public async Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            try
            {
                // Start building the query with the base DbSet<T>
                IQueryable<T> query = dbSet;
                // Include related entities if specified (for example, join with other tables)
                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                // Retrieve the first entity that matches the condition or return null if none found
                return await query.FirstOrDefaultAsync(expression);
            }
            catch (Exception e)
            {
                // Log the error and return null if any issue occurs
                _logger.LogError(e, "Error retrieving single entity by condition");
                return null;
            }
        }

        // Delete multiple entities that match a given condition
        public void DeleteMulti(Expression<Func<T, bool>> where)
        {
            try
            {
                // Find all entities that match the condition (where)
                var objects = dbSet.Where(where).ToList();

                // Remove each entity from the DbSet
                foreach (var obj in objects)
                {
                    dbSet.Remove(obj);
                }
            }
            catch (Exception e)
            {
                // Log any errors that occur during deletion
                _logger.LogError(e, "Error deleting multiple entities");
            }
        }

        // Count the number of entities that satisfy a given condition
        public async Task<int> Count(Expression<Func<T, bool>> where)
        {
            try
            {
                // Use CountAsync to return the number of entities that match the condition
                return await dbSet.CountAsync(where);
            }
            catch (Exception e)
            {
                // Log the error and return 0 if an issue arises
                _logger.LogError(e, "Error counting entities");
                return 0;
            }
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public virtual async Task<bool> BulkInsert(List<T> entity)
        {
            try
            {
                await _context.BulkInsertAsync(entity);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error bulk inserting entities");
                return false;
            }
        }

        public virtual async Task<bool> BulkUpdate(List<T> entity)
        {
            try
            {
                await _context.BulkUpdateAsync(entity);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error bulk updating entities");
                return false;
            }
        }
        public async Task<bool> RemoveRange(IEnumerable<T> entities)
        {
            try
            {
                dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error removing range of entities");
                return false;
            }
        }
    }
}