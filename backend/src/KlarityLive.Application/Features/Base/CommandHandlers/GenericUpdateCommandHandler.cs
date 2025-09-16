using KlarityLive.Application.Features.Base.Commands;
using KlarityLive.Domain.Entities.Base;
using KlarityLive.Domain.Exceptions;
using KlarityLive.Infrastructure.Data.Context;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KlarityLive.Application.Features.Base.CommandHandlers
{
    public class GenericUpdateCommandHandler<TEntity> : IRequestHandler<UpdateCommand<TEntity>, TEntity>
    where TEntity : BaseEntity
    {
        private readonly KlarityLiveDbContext _context;
        private readonly ILogger<GenericUpdateCommandHandler<TEntity>> _logger;

        public GenericUpdateCommandHandler(KlarityLiveDbContext context, ILogger<GenericUpdateCommandHandler<TEntity>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TEntity> Handle(UpdateCommand<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                var existingEntity = await _context.Set<TEntity>().FindAsync(new object[] { request.Id }, cancellationToken);
                if (existingEntity == null)
                    throw new NotFoundException($"{typeof(TEntity).Name} with ID {request.Id} not found");

                // Update properties using reflection
                UpdateEntityProperties(existingEntity, request.Entity);

                // Set update timestamp if entity supports it
                if (existingEntity is ITimestamped timestamped)
                {
                    timestamped.UpdatedDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("{EntityType} updated with ID: {Id}", typeof(TEntity).Name, request.Id);
                return existingEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating {EntityType} with ID: {Id}", typeof(TEntity).Name, request.Id);
                throw;
            }
        }

        private void UpdateEntityProperties(TEntity existing, TEntity updated)
        {
            var properties = typeof(TEntity).GetProperties()
                .Where(p => p.CanWrite && p.Name != "Id" && p.Name != "CreatedDate")
                .ToList();

            foreach (var property in properties)
            {
                var newValue = property.GetValue(updated);
                if (newValue != null)
                {
                    property.SetValue(existing, newValue);
                }
            }
        }
    }
}