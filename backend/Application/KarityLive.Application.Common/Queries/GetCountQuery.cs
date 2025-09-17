using KlarityLive.Domain.Core.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Common.Queries
{
    public class GetCountQuery<TEntity> : IRequest<int>
        where TEntity : BaseEntity
    {
        public Dictionary<string, object> Filters { get; set; } = new();
    }
}
