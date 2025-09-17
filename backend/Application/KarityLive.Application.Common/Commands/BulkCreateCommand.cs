using KlarityLive.Domain.Core.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Common.Commands
{
    public class BulkCreateCommand<TEntity> : IRequest<IEnumerable<TEntity>>
      where TEntity : BaseEntity
    {
        public IEnumerable<TEntity> Entities { get; set; }
    }

}
