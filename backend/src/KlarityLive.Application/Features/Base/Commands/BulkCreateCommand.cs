using KlarityLive.Domain.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Features.Base.Commands
{
    public class BulkCreateCommand<TEntity> : IRequest<IEnumerable<TEntity>>
      where TEntity : BaseEntity
    {
        public IEnumerable<TEntity> Entities { get; set; }
    }

}
