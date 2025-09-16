using KlarityLive.Application.Features.Base.Commands;
using KlarityLive.Domain.Entities.Base;
using KlarityLive.Infrastructure.Data.Context;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KlarityLive.Application.Features.Base.CommandHandlers
{
    public class GenericCreateCommandHandler<TEntity> : IRequestHandler<CreateCommand<TEntity>, TEntity>
     where TEntity : BaseEntity
    {
        private readonly KlarityLiveDbContext _context;
        private readonly ILogger<GenericCreateCommandHandler<TEntity>> _logger;

        public GenericCreateCommandHandler(KlarityLiveDbContext context, ILogger<GenericCreateCommandHandler<TEntity>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TEntity> Handle(CreateCommand<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = request.Entity;

                // Set timestamps if entity supports it
                if (entity is ITimestamped timestamped)
                {
                    timestamped.CreatedDate = DateTime.UtcNow;
                }

                _context.Set<TEntity>().Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("{EntityType} created with ID: {Id}", typeof(TEntity).Name, entity.Id);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }
    }
}
