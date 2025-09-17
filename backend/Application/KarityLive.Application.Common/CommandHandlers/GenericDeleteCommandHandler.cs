using KlarityLive.Application.Common.Commands;
using KlarityLive.Domain.Core.Entities.Base;
using KlarityLive.Infrastructure.Data.DbContexts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KlarityLive.Application.Common.CommandHandlers
{
    public class GenericDeleteCommandHandler<TEntity> : IRequestHandler<DeleteCommand<TEntity>, bool>
        where TEntity : BaseEntity
    {
        private readonly KlarityLiveDbContext _context;
        private readonly ILogger<GenericDeleteCommandHandler<TEntity>> _logger;

        public GenericDeleteCommandHandler(KlarityLiveDbContext context, ILogger<GenericDeleteCommandHandler<TEntity>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCommand<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Set<TEntity>().FindAsync(new object[] { request.Id }, cancellationToken);
                if (entity == null)
                    return false;

                if (request.SoftDelete && entity is ISoftDelete softDeleteEntity)
                {
                    softDeleteEntity.IsDeleted = true;
                    if (entity is ITimestamped timestamped)
                    {
                        timestamped.UpdatedOn = DateTime.UtcNow;
                    }
                }
                else
                {
                    _context.Set<TEntity>().Remove(entity);
                }

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("{EntityType} deleted with ID: {Id}", typeof(TEntity).Name, request.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting {EntityType} with ID: {Id}", typeof(TEntity).Name, request.Id);
                throw;
            }
        }
    }
}
