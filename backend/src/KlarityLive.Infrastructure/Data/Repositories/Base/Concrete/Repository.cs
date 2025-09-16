using EFCore.BulkExtensions;
using KlarityLive.Domain.Entities.Base;
using KlarityLive.Domain.Exceptions;
using KlarityLive.Infrastructure.Data.Context;
using KlarityLive.Infrastructure.Data.Repositories.Base.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace KlarityLive.Infrastructure.Data.Repositories.Base.Concrete
{
    public class Repository<T>(KlarityLiveDbContext context) : IRepository<T> where T : BaseEntity
    {
        internal readonly KlarityLiveDbContext Context = context;
        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddAsync(T entity)
        {

            if (entity != null)
            {
                entity.CreatedDateTime = DateTime.UtcNow;
                Context.Set<T>().Add(entity);

                try
                {
                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Handle concurrency exceptions if needed
                    throw new InvalidOperationException("The entity could not be added due to a concurrency issue.", ex);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(entity));
            }
        }

        /// <summary>
        /// Bulk insert entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task BulkAddAsync(List<T> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                throw new ArgumentException("The list of entities cannot be null or empty.", nameof(entities));
            }

            var now = DateTime.Now;
            foreach (var entity in entities)
            {
                entity.CreatedDateTime = now;
            }

            try
            {
                await Context.BulkInsertAsync(entities);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Optionally log the exception or rethrow it
                throw new InvalidOperationException("An error occurred while bulk adding entities.", ex);
            }
        }

        /// <summary>
        /// Bulk insert entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task BulkUpdateAsync(List<T> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                throw new ArgumentException("The list of entities cannot be null or empty.", nameof(entities));
            }

            var now = DateTime.Now;
            foreach (var entity in entities)
            {
                entity.CreatedDateTime = now;
            }

            try
            {
                await Context.BulkUpdateAsync(entities);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Optionally log the exception or rethrow it
                throw new InvalidOperationException("An error occurred while bulk adding entities.", ex);
            }
        }

        /// <summary>
        /// Bulk delete
        /// </summary>
        /// <param name="entities">List<T> entities</param>
        /// <returns></returns>
        public async Task BulkDeleteAsync(List<T> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                throw new ArgumentException("The list of entities cannot be null or empty.", nameof(entities));
            }

            try
            {
                await Context.BulkDeleteAsync(entities);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Optionally log the exception or rethrow it
                throw new InvalidOperationException("An error occurred while bulk deleting entities.", ex);
            }
        }

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteAsync(T entity)
        {
            if (entity != null)
            {
                try
                {
                    // Ensure the entity is being tracked by the context
                    if (Context.Entry(entity).State == EntityState.Detached)
                    {
                        Context.Set<T>().Attach(entity);
                    }

                    Context.Remove(entity);
                    await Context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Optionally log the exception or rethrow it
                    throw new InvalidOperationException("An error occurred while deleting the entity.", ex);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(entity));
            }
        }

        /// <summary>
        /// Gets all
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await Context.Set<T>()
                    .AsSplitQuery()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                throw new InvalidOperationException("An error occurred while retrieving all entities.", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves an entity from the repository based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
        public async Task<T> GetByIdAsync(int id, bool notracking = false)
        {
            T entity;

            try
            {
                if (notracking)
                {
                    entity = await Context.Set<T>().AsSplitQuery().AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
                }
                else
                {
                    entity = await Context.Set<T>().AsSplitQuery().FirstOrDefaultAsync(a => a.Id == id);
                }

                return entity == null ? throw new NotFoundException($"Entity of type {typeof(T).Name} with ID {id} was not found.") : entity;
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                throw new InvalidOperationException($"An error occurred while retrieving entity of type {typeof(T).Name} with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<T> UpdateAsync(T entity)
        {
            if (entity != null)
            {
                Context.Set<BaseEntity>().Update(entity);

                try
                {
                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Handle concurrency exceptions if needed
                    throw new InvalidOperationException("The entity was updated by another user.", ex);
                }

                return entity;
            }

            throw new ArgumentNullException(nameof(entity));
        }

        /// <summary>
        /// Seaches an entity
        /// </summary>
        /// <param name="predicate">Expression</param>
        /// <returns></returns>
        public async Task<List<T>> SearchAsync(Expression<Func<T, bool>> predicate)
        {

            try
            {
                return await Context.Set<T>()
                                    .Where(predicate)
                                    .AsSplitQuery()
                                    .ToListAsync();
            }
            catch (JsonReaderException jsonEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while searching for entities.", ex);
            }

        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {

            try
            {
                return await Context.Set<T>()
                                    .Where(predicate)
                                    .AsSplitQuery()
                                    .FirstOrDefaultAsync();
            }
            catch (JsonReaderException jsonEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while searching for entities.", ex);
            }

        }

        public IQueryable<T> Search(Expression<Func<T, bool>> predicate, bool asNoTracking = false)
        {
            var query = Context.Set<T>().Where(predicate);

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query.AsSplitQuery().AsQueryable();
        }
    }
}
