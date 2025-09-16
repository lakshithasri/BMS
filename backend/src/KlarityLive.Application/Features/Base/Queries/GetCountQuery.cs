using KlarityLive.Domain.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Features.Base.Queries
{
    public class GetCountQuery<TEntity> : IRequest<int>
        where TEntity : BaseEntity
    {
        public Dictionary<string, object> Filters { get; set; } = new();
    }
}
