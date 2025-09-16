using KlarityLive.Domain.Entities.Base;
using System.Linq.Expressions;

namespace KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces
{
    /// <summary>
    /// Represents a generic repository interface for managing basic CRUD operations on entities.
    /// </summary>
    /// <typeparam name="T">The type of entity for the repository.</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds a collection of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entity collection to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task BulkAddAsync(List<T> entities);

        /// <summary>
        /// Updates a collection of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entity collection to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task BulkUpdateAsync(List<T> entities);

        /// <summary>
        /// Deletes a collection of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entity collection to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task BulkDeleteAsync(List<T> entities);

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves an entity from the repository based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
        Task<T> GetByIdAsync(int id, bool notracking = false);

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Searches an entity
        /// </summary>
        /// <param name="predicate">Expression</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IQueryable of entities.</returns>
        Task<List<T>> SearchAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Searches an entity
        /// </summary>
        /// <param name="predicate">Expression</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IQueryable of entities.</returns>
        IQueryable<T> Search(Expression<Func<T, bool>> predicate, bool asNoTracking = false);

        /// <summary>
        /// Gets the first or default
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
